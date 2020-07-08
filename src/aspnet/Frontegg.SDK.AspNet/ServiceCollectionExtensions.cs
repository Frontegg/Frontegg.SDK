using System;
using System.Net.Http;
using Frontegg.SDK.AspNet.Authentication;
using Frontegg.SDK.AspNet.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Frontegg.SDK.AspNet
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection InnerAddFrontegg(IServiceCollection services,
            Action<FronteggOptions> options,
            Action<IHttpClientBuilder> httpClientBuilderAction)
        {
            services.TryAddSingleton<IAuthenticationStateStore, AuthenticationStateStore>();
            services.TryAddSingleton<IFronteggAuthenticator, FronteggAuthenticator>();
            services.TryAddSingleton<IFronteggHttpRequestProxySender, FronteggHttpRequestProxySender>();
            services.TryAddSingleton<IFronteggHttProxyRequestCreator, FronteggHttProxyRequestCreator>();
            services.TryAddSingleton<IProxyHttpResponseInjector, ProxyHttpResponseInjector>();
            services.TryAddSingleton<IConfigureOptions<FronteggOptions>, FronteggConfigurator>();
            services.TryAddSingleton<ICustomFronteggOptionsConfigurationProvider>(new CustomFronteggOptionsConfigurationProvider(options));
            services.TryAddSingleton<IFronteggProxyResponseCreator, FronteggProxyResponseCreator>();
            services.AddHostedService<FronteggAuthenticationJob>();

            var httpClientBuilder = services.AddHttpClient(Frontegg.ClientName)
                .ConfigureHttpClient((serviceProvider, clientBuilder) =>
                {
                    var url = serviceProvider.GetService<IOptions<FronteggOptions>>().Value.Url;
                    clientBuilder.BaseAddress = new Uri(url);
                    
                }).ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler
                {
                    AllowAutoRedirect = false,
                    UseCookies = false
                });
            
            httpClientBuilderAction?.Invoke(httpClientBuilder);
            
            return services;
        }
        public static IServiceCollection AddFrontegg(this IServiceCollection services,
            Action<FronteggOptions> options = null,
            Action<IHttpClientBuilder> httpClientBuilderAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            return InnerAddFrontegg(services, options, httpClientBuilderAction);
        }
    }
}