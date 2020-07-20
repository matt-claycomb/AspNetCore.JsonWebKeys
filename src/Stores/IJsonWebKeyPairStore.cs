using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using AspNetCore.JsonWebKeys.Data;

namespace AspNetCore.JsonWebKeys.Stores
{
    public interface IJsonWebKeyPairStore
    {
        ImmutableDictionary<string, IJsonWebKeyPair> GetAll();
        IJsonWebKeyPair Get(string name);
        bool Exists(string name);
        void Delete(string name);
        void Add(string name, IJsonWebKeyPair keyPair);
    }
}
