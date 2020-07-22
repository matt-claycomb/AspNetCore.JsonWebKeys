using System.Collections.Generic;
using AspNetCore.JsonWebKeys.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.JsonWebKeys
{
    public  class KeyController : ControllerBase
    {
        private readonly JsonWebKeyPairManagerService _jsonWebKeyManagerService;

        public KeyController(JsonWebKeyPairManagerService jsonWebKeyManagerService)
        {
            _jsonWebKeyManagerService = jsonWebKeyManagerService;
        }

        public IActionResult GetAll()
        {
            var lastKey = _jsonWebKeyManagerService.GetLastKey()?.PublicKey;
            var currentKey = _jsonWebKeyManagerService.GetCurrentKey().PublicKey;
            var nextKey = _jsonWebKeyManagerService.GetNextKey().PublicKey;

            var retVal = new List<JsonWebKey>();
            if (lastKey != null)
                retVal.Add(lastKey);
            retVal.AddRange(new [] {currentKey, nextKey});

            return Ok(retVal);
        }

        public IActionResult GetLast()
        {
            var lastKey = _jsonWebKeyManagerService.GetLastKey()?.PublicKey;

            if (lastKey == null)
                return NotFound();

            return Ok(lastKey);
        }

        public IActionResult GetCurrent()
        {
            return Ok(_jsonWebKeyManagerService.GetCurrentKey().PublicKey);
        }

        public IActionResult GetNext()
        {
            return Ok(_jsonWebKeyManagerService.GetNextKey().PublicKey);
        }
    }
}
