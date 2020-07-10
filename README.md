# Frontegg.SDK.AspNet

<img src="https://fronteggstuff.blob.core.windows.net/frongegg-logos/logo-transparent.png" alt="Frontegg">

##
Frontegg is a web platform where SaaS companies can set up their fully managed, scalable and brand aware - SaaS features and integrate them into their SaaS portals in up to 5 lines of code.

## Installation
`dotnet add package Frontegg.SDK.AspNet`

## Middleware
When using Frontegg's managed UI features and UI libraries, Frontegg allow simple integration via middleware usage

To use the Frontegg's middleware use the frontegg middleware from the `Frontegg.SDK.AspNet` Nuget

```c#
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        // When using AspNetCore DI, frontegg allows to inject the class to extract
        // the userId and tenatId.
        services.TryAddSingleton<IFronteggProxyInfoExtractor, MyFronteggProxyExtractor>();
        services.AddFrontegg();
     }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
            
        // When using the DI to inject the `IFronteggProxyInfoExtractor`,
        // you must indicate using generics the class injected.
        app.UseFrontegg<MyFronteggProxyExtractor>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
      }
    }
    
    // create your own class that implements `IFronteggProxyInfoExtractor` to extract 
    // your userId and tenantId
    public class MyFronteggProxyExtractor : IFronteggProxyInfoExtractor
    {
        public Task<FronteggTenantInfo> Extract(HttpRequest request)
        {
            return Task.FromResult(new FronteggTenantInfo()
            {
                UserId = "userId",
                TenantId = "tenantId"
            });
        }
    }
}
```

## Configuration
Frontegg middleware requires `ApiKey` and `ClientId` which is given to you in [Frontegg Portal administration tag](https://portal.frontegg.com/administration).

```c#
services.AddFrontegg(options =>
        {
            options.ApiKey = "apiKey";
            options.ClientId = "clientId";
            options.ThrowOnMissingConfiguration = true;
        });
```

It is also possible to use `appsetting.json` to pass configuration variables like so:
```json
"Frontegg": {
    "ApiKey" : "apiKey",
    "ClientId": "clientId",
    "ThrowOnMissingConfiguration": false
}
```

`ThrowOnMissingConfiguration` if set on true, frontegg middleware will throw an `OptionsValidationException` when `ApiKey` or `ClientId` are missing.
If set on `false` frontegg middleware will not proxy middleware UI data.

## Usage
Frontegg allows to pass `userId` and `tenatId` as configured in [Frontegg Portal](https://portal.frontegg.com/). So Frontegg middleware has two options to extract the data.
* Using a class implementing `IFronteggProxyInfoExtractor` interface and inject it to AspNetCore DI.
* Using a delegate to extract the data.

### IFronteggProxyInfoExtractor
When implementing `IFronteggProxyInfoExtractor` the result should be `FronteggTenantInfo` this will be passed into Frontegg http request
```c#
public class MyFronteggProxyExtractor : IFronteggProxyInfoExtractor
{
    public Task<FronteggTenantInfo> Extract(HttpRequest request)
    {
        return Task.FromResult(new FronteggTenantInfo()
        {
            UserId = "userId",
            TenantId = "tenantId"
        });
    }
}
```
Please note that in order for AspNetCore to resolve the implementation of `IFronteggProxyInfoExtractor` you must register like so:
```c#
services.TryAddSingleton<IFronteggProxyInfoExtractor, MyFronteggProxyExtractor>();
```
When indicating AspNetCore to use Frontegg in the pipeline mark the implementation class using generics like so:
```c#
app.UseFrontegg<MyFronteggProxyExtractor>();
```

### delegate

If not using the method above frontegg allows you to use delegate in order to extract `FronteggTenantInfo`

```c#
app.UseFrontegg(request =>
            {
                //...
                return Task.FromResult(new FronteggTenantInfo()
                {
                    ...
                });
            });
```
