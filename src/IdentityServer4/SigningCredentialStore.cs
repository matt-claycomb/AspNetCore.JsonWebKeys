using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.JsonWebKeys.Services;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.JsonWebKeys.IdentityServer4
{
    class SigningCredentialStore : ISigningCredentialStore
    {
        private readonly JsonWebKeyPairManagerService _keyPairManagerService;

        public SigningCredentialStore(JsonWebKeyPairManagerService keyPairManagerService)
        {
            _keyPairManagerService = keyPairManagerService;
        }

        public Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            //TODO Refactor for proper async
            return Task.FromResult(_keyPairManagerService.GetCurrentKey().SigningCredentials);
        }
    }
}
