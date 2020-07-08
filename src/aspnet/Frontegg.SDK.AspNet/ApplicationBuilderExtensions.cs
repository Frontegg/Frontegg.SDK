using System.Collections.Generic;
using Frontegg.SDK.AspNet.Proxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frontegg.SDK.AspNet
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFrontegg<TFronteggProxyInfoExtractor>(this IApplicationBuilder app) where TFronteggProxyInfoExtractor: IFronteggProxyInfoExtractor
        {
            ValidateOptions(app);

            app.Map("/frontegg",appBuilder =>
            {
                app.UseMiddleware<FronteggProxyMiddleware<TFronteggProxyInfoExtractor>>();
            });
            
            return app;
        }
        
        public static IApplicationBuilder UseFrontegg(this IApplicationBuilder app, HandleProxyRequest handleProxyRequest)
        {
            ValidateOptions(app);

            app.Map("/frontegg",appBuilder =>
            {
                app.UseMiddleware<FronteggProxyMiddleware<HandleProxyRequestWrapper>>(new HandleProxyRequestWrapper(handleProxyRequest));
            });
            
            return app;
        }

        private static void ValidateOptions(IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<FronteggOptions>>().Value;

            var errors = new List<string>(2);

            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                errors.Add("Frontegg ApiKey is not set."); // give link to documents
            }
            
            if (string.IsNullOrWhiteSpace(options.Url))
            {
                errors.Add("Frontegg Url is not set."); // give link to documents
            }
            
            if(string.IsNullOrWhiteSpace(options.ClientId))
            {
                errors.Add("Frontegg ClientId is not set."); // give link to documents
            }

            if (errors.Count == 0)
            {
                options.IsEnabled = true;
                return;
            }
            
            if(options.ThrowOnMissingConfiguration)
                throw new OptionsValidationException(nameof(FronteggOptions), typeof(FronteggOptions), errors);

            options.IsEnabled = false;
        }
    }
}