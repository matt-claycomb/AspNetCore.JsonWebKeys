using System.Collections.Generic;
using AspNetCore.JsonWebKeys.Data;

namespace AspNetCore.JsonWebKeys.Factories
{
    public class RsaJsonWebKeyPairFactory : IJsonWebKeyPairFactory
    {
        public IJsonWebKeyPair Create()
        {
            return RsaJsonWebKeyPair.Create();
        }

        public IJsonWebKeyPair Deserialize(Dictionary<string, string> data)
        {
            return RsaJsonWebKeyPair.Deserialize(data);
        }

        public Dictionary<string, string> Serialize(IJsonWebKeyPair keyPair)
        {
            return RsaJsonWebKeyPair.Serialize(keyPair as RsaJsonWebKeyPair);
        }
    }
}
