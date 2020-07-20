using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AspNetCore.JsonWebKeys.Data;
using AspNetCore.JsonWebKeys.Factories;
using AspNetCore.JsonWebKeys.Options;

namespace AspNetCore.JsonWebKeys.Stores
{
    public class FileJsonWebKeyPairStore : IJsonWebKeyPairStore
    {
        private readonly IJsonWebKeyPairFactory _jsonWebKeyPairFactory;
        private Dictionary<string, IJsonWebKeyPair> _keyPairs;
        private readonly string _filename;

        private void ReadFile()
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_filename));
            _keyPairs = data.ToDictionary(kp => kp.Key, kp => _jsonWebKeyPairFactory.Deserialize(kp.Value));
        }

        private void WriteFile()
        {
            var data = _keyPairs.ToDictionary(kp => kp.Key, kp => _jsonWebKeyPairFactory.Serialize(kp.Value));
            File.WriteAllText(_filename, JsonSerializer.Serialize(data));
        }

        public FileJsonWebKeyPairStore(IJsonWebKeyPairFactory jsonWebKeyPairFactory, FileJsonWebKeyPairStoreOptions options)
        {
            _jsonWebKeyPairFactory = jsonWebKeyPairFactory;
            _filename = options.Filename;
            ReadFile();
        }

        public ImmutableDictionary<string, IJsonWebKeyPair> GetAll()
        {
            return _keyPairs.ToImmutableDictionary();
        }

        public bool Exists(string name)
        {
            return _keyPairs.ContainsKey(name);
        }

        public IJsonWebKeyPair Get(string name)
        {
            return _keyPairs[name];
        }

        public void Delete(string name)
        {
            _keyPairs.Remove(name);
            WriteFile();
        }

        public void Add(string name, IJsonWebKeyPair keyPair)
        {
            _keyPairs.Add(name, keyPair);
            WriteFile();
        }
    }
}
