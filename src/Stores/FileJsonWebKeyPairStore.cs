using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AspNetCore.JsonWebKeys.Options;

namespace AspNetCore.JsonWebKeys.Stores
{
    public class FileJsonWebKeyPairStore : IJsonWebKeyPairStore
    {
        private readonly string _filename;

        public FileJsonWebKeyPairStore(FileJsonWebKeyPairStoreOptions options)
        {
            _filename = options.Filename;
        }

        public Dictionary<string, Dictionary<string, string>> Load()
        {
            if (File.Exists(_filename))
                return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(_filename));
            else
                return new Dictionary<string, Dictionary<string, string>>();
        }

        public void Save(Dictionary<string, Dictionary<string, string>> data)
        {
            File.WriteAllText(_filename, JsonSerializer.Serialize(data));
        }
    }
}
