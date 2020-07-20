using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.JsonWebKeys.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.JsonWebKeys.Factories
{
    public class RsaJsonWebKeyPairFactory : IJsonWebKeyPairFactory
    {
        public IJsonWebKeyPair Create()
        {
            return RsaJsonWebKeyPair.Create();
        }

        public IJsonWebKeyPair Deserialize(string data)
        {
            return RsaJsonWebKeyPair.Deserialize(data);
        }

        public string Serialize(IJsonWebKeyPair keyPair)
        {
            return RsaJsonWebKeyPair.Serialize(keyPair as RsaJsonWebKeyPair);
        }
    }
}
