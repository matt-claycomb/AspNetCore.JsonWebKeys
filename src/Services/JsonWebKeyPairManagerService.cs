using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.JsonWebKeys.Data;
using AspNetCore.JsonWebKeys.Factories;
using AspNetCore.JsonWebKeys.Options;
using AspNetCore.JsonWebKeys.Stores;
using Microsoft.Extensions.Logging;

namespace AspNetCore.JsonWebKeys.Services
{
    public class JsonWebKeyPairManagerService
    {
        private readonly IJsonWebKeyPairFactory _jsonWebKeyPairFactory;
        private readonly IJsonWebKeyPairStore _jsonWebKeyPairStore;
        private readonly JsonWebKeyPairManagerOptions _options;
        private readonly ILogger _logger;
        public JsonWebKeyPairManagerService(IJsonWebKeyPairFactory jsonWebKeyPairFactory, IJsonWebKeyPairStore jsonWebKeyPairStore, ILogger logger, JsonWebKeyPairManagerOptions options)
        {
            _jsonWebKeyPairFactory = jsonWebKeyPairFactory;
            _jsonWebKeyPairStore = jsonWebKeyPairStore;
            _logger = logger;
            _options = options;

            if (!_jsonWebKeyPairStore.Exists("current"))
            {
                _jsonWebKeyPairStore.Add("current", jsonWebKeyPairFactory.Create(_options.KeyLifetimeDays));
            }

            if (!_jsonWebKeyPairStore.Exists("next"))
            {
                _jsonWebKeyPairStore.Add("next", jsonWebKeyPairFactory.Create(_options.KeyLifetimeDays));
            }
        }

        public IJsonWebKeyPair GetLastKey()
        {
            return _jsonWebKeyPairStore.Exists("last") ? _jsonWebKeyPairStore.Get("last") : null;
        }

        public IJsonWebKeyPair GetCurrentKey()
        {
            return _jsonWebKeyPairStore.Get("current");
        }

        public IJsonWebKeyPair GetNextKey()
        {
            return _jsonWebKeyPairStore.Get("next");
        }

        private void RotateKeys()
        {
            var newLastKey = _jsonWebKeyPairStore.Exists("current") ? _jsonWebKeyPairStore.Get("current") : null;
            var newCurrentKey = _jsonWebKeyPairStore.Exists("next")
                ? _jsonWebKeyPairStore.Get("next")
                : _jsonWebKeyPairFactory.Create(_options.KeyLifetimeDays);
            var newNextKey = _jsonWebKeyPairFactory.Create(_options.KeyLifetimeDays);
        }
    }
}
