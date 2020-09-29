# ASP.Net Core 3.1 JSON Web Keys

This library contains services designed to help generate, store and manage JSON Web Keys, such as those used for OAuth2 client assertions.

#!!! Do not use right now! There is an error in the application logic meaning the key IDs in the jwks endpoint change, which they should not, per the OAuth2 specification. This could cause major issues depending on implementation.


## Limitations

This may or may not be usable as-is in production due to the following limitations:
- No check for available entropy is completed.
- The built-in store doesn't encrypt keys in storage.

## Usage

### Install via NuGet

To install AspNetCore.JsonWebKeys, run the following command in the Package Manager Console:

[![Nuget](https://img.shields.io/nuget/v/AspNetCore.JsonWebKeys)](https://github.com/matt-claycomb/AspNetCore.JsonWebKeys)

```powershell
PM> Install-Package AspNetCore.JsonWebKeys
```

You can also view the [package page](http://www.nuget.org/packages/AspNetCore.JsonWebKeys/) on NuGet.

### Configuration

Most of the setup is completed in the ConfigureServices method, with optional route registration taking place inside of IApplicationBuilder.UseEndpoints.

```csharp
public void ConfigureServices(IServiceCollection services) {
	services.AddJsonWebKeyManagement<RsaJsonWebKeyPairFactory>(
		(provider, options) => {
			options.KeyLifetimeDays = 90;
		}
	).AddFileStore(
		(provider, options) => {
			options.Filename = "keys.json"
		}
	);
}
```

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	app.UseEndpoints(
		endpoints => {
			endpoints.MapControllers();
			endpoints.RegisterJsonWebKeyRoutes();
		}
	);
}
```

To change what type of key is being generated and stored, a new implementation of IJsonWebKeyPair and IJsonWebKeyPairFactory should be written, and the new implementation of IJsonWebKeyPairFactory should be passed as the argument to IServiceCollection.AddJsonWebKeyManagement.

### Implementation

The registered services automatically manage issuing and rotating keys on a regular interval, so usage only requires injecting  JsonWebKeyPairManagerService into any services that need to access the keys.

Keys are accessible through `/.well-known/openid-configuration/jwks` and `/.well-known/openid-configuration/jwks/{last,current,next}` for any ASP.Net core applications which register the built-in routes.
