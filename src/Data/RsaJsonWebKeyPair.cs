using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using IdentityServer4.Models;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = Microsoft.IdentityModel.Tokens.JsonWebKey;

namespace AspNetCore.JsonWebKeys.Data
{
    public class RsaJsonWebKeyPair : IJsonWebKeyPair
    {
        public JsonWebKey PrivateKey { get; }
        public JsonWebKey PublicKey { get; }
        public DateTime CreatedTime { get; }
        public SigningCredentials SigningCredentials { get; }
        public SecurityKeyInfo SecurityKeyInfo { get; }

        private readonly string _privateKeyXml;

        private RsaJsonWebKeyPair(string privateKeyXml)
        {
            CreatedTime = DateTime.Now;

            _privateKeyXml = privateKeyXml;

            var rsaPrivateKey = RSA.Create();
            rsaPrivateKey.FromXmlString(privateKeyXml);

            var rsaPublicKey = RSA.Create();
            rsaPublicKey.FromXmlString(rsaPrivateKey.ToXmlString(false));
            
            var privateRsaSecurityKey = new RsaSecurityKey(rsaPrivateKey);

            PrivateKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(privateRsaSecurityKey);
            PublicKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(new RsaSecurityKey(rsaPublicKey));

            SigningCredentials = new SigningCredentials(privateRsaSecurityKey, PrivateKey.Alg);
            SecurityKeyInfo = new SecurityKeyInfo
            {
                Key = privateRsaSecurityKey,
                SigningAlgorithm = PrivateKey.Alg
            };
        }

        internal static RsaJsonWebKeyPair Create()
        {
            var rsaPrivateKey = RSA.Create();
            rsaPrivateKey.KeySize = 4096;

            return new RsaJsonWebKeyPair(rsaPrivateKey.ToXmlString(true));
        }

        internal static RsaJsonWebKeyPair Deserialize(string data)
        {
            return new RsaJsonWebKeyPair(data);
        }

        internal static string Serialize(RsaJsonWebKeyPair keyPair)
        {
            return keyPair._privateKeyXml;
        }
    }
}
