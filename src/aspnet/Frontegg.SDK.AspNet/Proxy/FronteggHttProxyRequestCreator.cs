using System;
using System.Net.Http;
using Frontegg.SDK.AspNet.Http;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class FronteggHttProxyRequestCreator : IFronteggHttProxyRequestCreator
    {
        public HttpRequestMessage CreateProxyRequest(HttpContext context, ProxyHttpRequestData data)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (data == null) throw new ArgumentNullException(nameof(data));
            var proxyRequest = context.Request.CreateProxyHttpRequest();
            var relativeUrl = context.Request.GetFronteggOriginPathAndQuery();
            proxyRequest.RequestUri = relativeUrl;
            PopulateFronteggData(proxyRequest, data);
            proxyRequest.Headers.Host = data.FronteggUrl.Authority;
            return proxyRequest;
        }
        
        private static void PopulateFronteggData(HttpRequestMessage proxyRequest, ProxyHttpRequestData data)
        {
            if (proxyRequest == null) throw new ArgumentNullException(nameof(proxyRequest));
            proxyRequest.Headers.Add(Frontegg.AccessTokenHeaderKey, data.Token);
            proxyRequest.Headers.Add(Frontegg.TenantIdHeaderKey, data.TenantId);
            proxyRequest.Headers.Add(Frontegg.UserIdHeaderKey, data.UserId);
        }
    }
}