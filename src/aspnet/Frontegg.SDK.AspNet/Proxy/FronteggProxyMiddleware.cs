using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class FronteggProxyMiddleware<TFronteggProxyInfoExtractor>
        where TFronteggProxyInfoExtractor : IFronteggProxyInfoExtractor
    {
        
        private readonly IProxyHttpResponseInjector _httpResponseInjector;
        private readonly IFronteggProxyResponseCreator _proxyResponseCreator;

        public FronteggProxyMiddleware(RequestDelegate _,
            IProxyHttpResponseInjector httpResponseInjector,
            IFronteggProxyResponseCreator proxyResponseCreator)
        {
            _httpResponseInjector = httpResponseInjector;
            _proxyResponseCreator = proxyResponseCreator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var response = await _proxyResponseCreator.GenerateProxyResponse(context).ConfigureAwait(false))
            {
                await _httpResponseInjector.InjectResponse(context, response).ConfigureAwait(false);
            }
        }
    }
}