using System;
using System.Net.Http;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Authentication;
using Frontegg.SDK.AspNet.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class FronteggProxyResponseCreator : IFronteggProxyResponseCreator
    {
        private readonly FronteggOptions _options;
        private readonly IAuthenticationStateStore _authenticationStateStore;
        private readonly IFronteggProxyInfoExtractor _proxyInfoExtractor;
        private readonly IFronteggHttpRequestProxySender _fronteggHttpRequestProxySender;
        private readonly IFronteggHttProxyRequestCreator _fronteggHttProxyRequestCreator;
        
        public FronteggProxyResponseCreator(IOptions<FronteggOptions> options,
            IAuthenticationStateStore authenticationStateStore,
            IFronteggProxyInfoExtractor proxyInfoExtractor,
            IFronteggHttpRequestProxySender fronteggHttpRequestProxySender,
            IFronteggHttProxyRequestCreator fronteggHttProxyRequestCreator)
        {
            _authenticationStateStore = authenticationStateStore;
            _proxyInfoExtractor = proxyInfoExtractor;
            _fronteggHttpRequestProxySender = fronteggHttpRequestProxySender;
            _fronteggHttProxyRequestCreator = fronteggHttProxyRequestCreator;
            _options = options.Value;
        }

        public async Task<HttpResponseMessage> GenerateProxyResponse(HttpContext context)
        {
            var authenticationResult = _authenticationStateStore.GetLatestState();

            if (!authenticationResult.IsAuthenticated)
            {
                return new ForbiddenHttpResponseMessage();
            }

            var tenantInfo = await _proxyInfoExtractor.Extract(context.Request)
                .ConfigureAwait(false);

            var data = new ProxyHttpRequestData
            {
                UserId = tenantInfo.UserId,
                TenantId = tenantInfo.TenantId,
                Token = authenticationResult.Token,
                FronteggUrl = new Uri(_options.Url)
            };

            var request = _fronteggHttProxyRequestCreator.CreateProxyRequest(context, data);

            var response = await _fronteggHttpRequestProxySender.ProxyHttpRequest(request, context)
                .ConfigureAwait(false);
            
            return response;
        }
    }
}