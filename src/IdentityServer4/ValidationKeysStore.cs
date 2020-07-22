using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.JsonWebKeys.Services;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace AspNetCore.JsonWebKeys.IdentityServer4
{
    public class ValidationKeysStore : IValidationKeysStore
    {
        private readonly JsonWebKeyPairManagerService _keyPairManagerService;

        public ValidationKeysStore(JsonWebKeyPairManagerService keyPairManagerService)
        {
            _keyPairManagerService = keyPairManagerService;
        }

        public Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            var keyInfo = new List<SecurityKeyInfo> {_keyPairManagerService.GetNextKey().SecurityKeyInfo};

            if (_keyPairManagerService.GetLastKey() != null)
            {
                keyInfo.Add(_keyPairManagerService.GetLastKey().SecurityKeyInfo);
            }

            return Task.FromResult(keyInfo.AsEnumerable());
        }
    }
}
