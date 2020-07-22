using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.JsonWebKeys.Data;
using AspNetCore.JsonWebKeys.Factories;
using AspNetCore.JsonWebKeys.Options;
using AspNetCore.JsonWebKeys.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.JsonWebKeys.Services
{
    public class JsonWebKeyPairManagerService
    {
        private readonly IJsonWebKeyPairFactory _jsonWebKeyPairFactory;
        private readonly IJsonWebKeyPairStore _jsonWebKeyPairStore;
        private Dictionary<string, IJsonWebKeyPair> _keyPairs;
        private readonly JsonWebKeyPairManagerOptions _options;
        private readonly ILogger _logger;

        public JsonWebKeyPairManagerService(IJsonWebKeyPairFactory jsonWebKeyPairFactory, IJsonWebKeyPairStore jsonWebKeyPairStore, IServiceProvider serviceProvider)
        {
            _jsonWebKeyPairFactory = jsonWebKeyPairFactory;
            _jsonWebKeyPairStore = jsonWebKeyPairStore;
            _options = serviceProvider.GetService<JsonWebKeyPairManagerOptions>();
            _logger = serviceProvider.GetService<ILogger<JsonWebKeyPairManagerService>>();

            ReloadFromStore();

            if (!_keyPairs.ContainsKey("current"))
            {
                _logger?.LogDebug("Generating non-existent \"current\" key.");
                _keyPairs.Add("current", jsonWebKeyPairFactory.Create());
            }

            if (!_keyPairs.ContainsKey("next"))
            {
                _logger?.LogDebug("Generating non-existent \"next\" key.");
                _keyPairs.Add("next", jsonWebKeyPairFactory.Create());
            }

            SaveToStore();
        }

        public void ReloadFromStore()
        {
            _keyPairs = _jsonWebKeyPairStore.Load().ToDictionary(d => d.Key, d => _jsonWebKeyPairFactory.Deserialize(d.Value));
        }

        private void SaveToStore()
        {
            var stringData = _keyPairs.ToDictionary(d => d.Key, d => _jsonWebKeyPairFactory.Serialize(d.Value));
            _jsonWebKeyPairStore.Save(stringData);
        }

        public IJsonWebKeyPair GetLastKey()
        {
            return _keyPairs.ContainsKey("last") ? _keyPairs["last"] : null;
        }

        public IJsonWebKeyPair GetCurrentKey()
        {
            return _keyPairs["current"];
        }

        public IJsonWebKeyPair GetNextKey()
        {
            return _keyPairs["current"];
        }

        internal void RotateKeys()
        {
            ReloadFromStore();
            
            if (_keyPairs["current"].CreatedTime.AddDays(_options.KeyLifetimeDays) < DateTime.Now)
            {
                _logger?.LogDebug("Rotating keys.");

                var newKeys = new Dictionary<string, IJsonWebKeyPair>
                {
                    {"last", _keyPairs["current"]},
                    {"current", _keyPairs["next"]},
                    {"next", _jsonWebKeyPairFactory.Create()}
                };

                _keyPairs = newKeys;

                SaveToStore();

                _logger.LogDebug("Done rotating keys.");
            }
            else
            {
                _logger?.LogDebug("Keys have recently been rotated. Skipping rotate.");
            }
        }
    }
}
