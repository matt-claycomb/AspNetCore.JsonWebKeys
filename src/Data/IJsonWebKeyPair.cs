using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Models;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = Microsoft.IdentityModel.Tokens.JsonWebKey;

namespace AspNetCore.JsonWebKeys.Data
{
    public interface IJsonWebKeyPair
    {
        JsonWebKey PrivateKey { get; }
        JsonWebKey PublicKey { get; }
        DateTime CreatedTime { get; }
        SigningCredentials SigningCredentials { get; }
        SecurityKeyInfo SecurityKeyInfo { get; }
    }
}
