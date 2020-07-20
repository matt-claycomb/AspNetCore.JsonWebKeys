using System;
using System.IdentityModel.Tokens;
using AspNetCore.JsonWebKeys.Data;
using AspNetCore.JsonWebKeys.Factories;
using AspNetCore.JsonWebKeys.IdentityServer4;
using AspNetCore.JsonWebKeys.Options;
using AspNetCore.JsonWebKeys.Services;
using AspNetCore.JsonWebKeys.Stores;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.JsonWebKeys
{
    public static class Extensions
    {
        public static JsonWebKeyManagementBuilder AddJsonWebKeyManagement(this IServiceCollection services, JsonWebKeyType jsonWebKeyType, Func<IServiceProvider, JsonWebKeyPairManagerOptions> optionsAction)
        {
            services.AddSingleton(optionsAction);

            switch (jsonWebKeyType)
            {
                case JsonWebKeyType.Rsa:
                    services.AddSingleton<IJsonWebKeyPairFactory, RsaJsonWebKeyPairFactory>();
                    services.AddSingleton<JsonWebKeyPairManagerService>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(jsonWebKeyType), jsonWebKeyType, null);
            }

            return new JsonWebKeyManagementBuilder(services);
        }

        public class JsonWebKeyManagementBuilder
        {
            private readonly IServiceCollection _serviceCollection;

            public JsonWebKeyManagementBuilder(IServiceCollection serviceCollection)
            {
                _serviceCollection = serviceCollection;
            }

            public JsonWebKeyManagementBuilder AddIdentityServerSupport()
            {
                _serviceCollection.AddSingleton<ISigningCredentialStore, SigningCredentialStore>();
                _serviceCollection.AddSingleton<IValidationKeysStore, ValidationKeysStore>();

                return this;
            }

            public JsonWebKeyManagementBuilder AddStore<T>() where T : class, IJsonWebKeyPairStore
            {
                _serviceCollection.AddSingleton<IJsonWebKeyPairStore, T>();

                return this;
            }

            public JsonWebKeyManagementBuilder AddFileStore(
                Func<IServiceProvider, FileJsonWebKeyPairStoreOptions> optionsAction)
            {
                _serviceCollection.AddSingleton(optionsAction);
                AddStore<FileJsonWebKeyPairStore>();

                return this;
            }
        }
    }
}
