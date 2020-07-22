using System;
using AspNetCore.JsonWebKeys.Factories;
using AspNetCore.JsonWebKeys.IdentityServer4;
using AspNetCore.JsonWebKeys.Options;
using AspNetCore.JsonWebKeys.Services;
using AspNetCore.JsonWebKeys.Stores;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.JsonWebKeys
{
    public static class Extensions
    {
        public static JsonWebKeyManagementBuilder AddJsonWebKeyManagement<T>(this IServiceCollection services, Action<IServiceProvider, JsonWebKeyPairManagerOptions> optionsAction = null) where T : class, IJsonWebKeyPairFactory
        {
            services.AddSingleton(provider =>
            {
                var options = new JsonWebKeyPairManagerOptions();

                optionsAction?.Invoke(provider, options);

                return options;
            });

            services.AddControllers().AddApplicationPart(typeof(KeyController).Assembly).AddControllersAsServices();
            services.AddSingleton<IJsonWebKeyPairFactory, T>();
            services.AddSingleton<JsonWebKeyPairManagerService>();
            services.AddHostedService<JsonWebKeyPairRotationService>();
            services.AddSingleton<ISigningCredentialStore, SigningCredentialStore>();
            services.AddSingleton<IValidationKeysStore, ValidationKeysStore>();

            return new JsonWebKeyManagementBuilder(services);
        }

        public static void RegisterJsonWebKeyRoutes(this IEndpointRouteBuilder builder)
        {
            builder.MapControllerRoute("jsonWebKey", "/.well-known/openid-configuration/jwks", new {controller = "Key", action = "GetAll"});
            builder.MapControllerRoute("jsonWebKeyLast", "/.well-known/openid-configuration/jwks/last", new { controller = "Key", action = "GetLast" });
            builder.MapControllerRoute("jsonWebKeyCurrent", "/.well-known/openid-configuration/jwks/current", new { controller = "Key", action = "GetCurrent" });
            builder.MapControllerRoute("jsonWebKeyNext", "/.well-known/openid-configuration/jwks/next", new { controller = "Key", action = "GetNext" });
        }

        public class JsonWebKeyManagementBuilder
        {
            private readonly IServiceCollection _serviceCollection;

            public JsonWebKeyManagementBuilder(IServiceCollection serviceCollection)
            {
                _serviceCollection = serviceCollection;
            }

            public JsonWebKeyManagementBuilder AddFileStore(
                Action<IServiceProvider, FileJsonWebKeyPairStoreOptions> optionsAction = null)
            {
                _serviceCollection.AddSingleton(provider =>
                {
                    var options = new FileJsonWebKeyPairStoreOptions();

                    optionsAction?.Invoke(provider, options);

                    return options;
                });

                _serviceCollection.AddSingleton<IJsonWebKeyPairStore, FileJsonWebKeyPairStore>();

                return this;
            }
        }
    }
}
