using System.Collections.Generic;

namespace AspNetCore.JsonWebKeys.Stores
{
    public interface IJsonWebKeyPairStore
    {
        Dictionary<string, Dictionary<string, string>> Load();
        void Save(Dictionary<string, Dictionary<string, string>> data);
    }
}
