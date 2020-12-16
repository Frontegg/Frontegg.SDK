using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Owin.Authentication;
using Frontegg.SDK.AspNet.Owin.Http;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    internal class FronteggProxyResponseCreator : IFronteggProxyResponseCreator
    {
        private readonly FronteggOptions _options;
        private readonly IAuthenticationStateStore _authenticationStateStore;
        private readonly IFronteggProxyInfoExtractor _proxyInfoExtractor;
        private readonly IFronteggHttpRequestProxySender _fronteggHttpRequestProxySender;
        private readonly IFronteggHttProxyRequestCreator _fronteggHttProxyRequestCreator;

        public FronteggProxyResponseCreator(FronteggOptions options) 
            : this(options,
                new AuthenticationStateStore(options),
                options.FronteggProxyInfoExtractor,
                new FronteggHttpRequestProxySender(options),
                new FronteggHttProxyRequestCreator())
        {}
        
        internal FronteggProxyResponseCreator(FronteggOptions options,
            IAuthenticationStateStore authenticationStateStore,
            IFronteggProxyInfoExtractor proxyInfoExtractor,
            IFronteggHttpRequestProxySender fronteggHttpRequestProxySender,
            IFronteggHttProxyRequestCreator fronteggHttProxyRequestCreator)
        {
            _authenticationStateStore = authenticationStateStore;
            _proxyInfoExtractor = proxyInfoExtractor;
            _fronteggHttpRequestProxySender = fronteggHttpRequestProxySender;
            _fronteggHttProxyRequestCreator = fronteggHttProxyRequestCreator;
            _options = options;
        }

        public async Task<HttpResponseMessage> GenerateProxyResponse(IOwinContext context)
        {
            var authenticationResult = await _authenticationStateStore.GetLastState();

            if (!authenticationResult.IsAuthenticated)
            {
                return new ForbiddenHttpResponseMessage();
            }

            var tenantInfo = _proxyInfoExtractor.Extract(context.Request);
            
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

        public class ForbiddenHttpResponseMessage : HttpResponseMessage
        {
            private const string Response =
                @"{""statusCode"":401,""error"":""Unauthorized"",""message"":""Could not verify vendor""}";

            public ForbiddenHttpResponseMessage()
                : base(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(Response);
            }
        }
    }
}