using System.Collections.Generic;
using AspNetCore.JsonWebKeys.Data;

namespace AspNetCore.JsonWebKeys.Factories
{
    public interface IJsonWebKeyPairFactory
    {
        IJsonWebKeyPair Create();
        IJsonWebKeyPair Deserialize(Dictionary<string, string> data);
        Dictionary<string, string> Serialize(IJsonWebKeyPair keyPair);
    }
}
