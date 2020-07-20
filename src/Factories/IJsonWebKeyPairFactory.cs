using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using AspNetCore.JsonWebKeys.Data;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.JsonWebKeys.Factories
{
    public interface IJsonWebKeyPairFactory
    {
        IJsonWebKeyPair Create();
        IJsonWebKeyPair Deserialize(string data);
        string Serialize(IJsonWebKeyPair keyPair);
    }
}
