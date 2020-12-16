using System;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Owin.Proxy;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin
{
    public class FronteggMiddleware
    {
        private readonly IFronteggProxyResponseCreator _proxyResponseCreator;
        private readonly IProxyHttpResponseInjector _proxyHttpResponseInjector;
        private FronteggOptions Options { get; }

        public FronteggMiddleware(FronteggOptions options) 
            : this(options, new FronteggProxyResponseCreator(options), new ProxyHttpResponseInjector())
        { }

        internal FronteggMiddleware(FronteggOptions options, IFronteggProxyResponseCreator proxyResponseCreator,
            IProxyHttpResponseInjector proxyHttpResponseInjector)
        {
            _proxyResponseCreator = proxyResponseCreator;
            _proxyHttpResponseInjector = proxyHttpResponseInjector;
            ValidateOptions(options);
            Options = options;
        }
        
        public async Task Invoke(IOwinContext context)
        {
            using (var response = await _proxyResponseCreator.GenerateProxyResponse(context).ConfigureAwait(false))
            {
                await _proxyHttpResponseInjector.InjectResponse(context, response).ConfigureAwait(false);
            }
        }
        
        private static void ValidateOptions(FronteggOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                throw new Exception("Frontegg ApiKey is not set.");
            }
            
            if (string.IsNullOrWhiteSpace(options.Url))
            {
                throw new Exception("Frontegg Url is not set.");
            }
            
            if(string.IsNullOrWhiteSpace(options.ClientId))
            {
                throw new Exception("Frontegg ClientId is not set.");
            }

            if (options.FronteggProxyInfoExtractor == null)
            {
                throw new Exception("FronteggProxyInfoExtractor is not set.");
            }
            
            options.IsEnabled = true;
        }
    }
}