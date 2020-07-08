using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal interface IFronteggHttProxyRequestCreator
    {
        HttpRequestMessage CreateProxyRequest(HttpContext context, ProxyHttpRequestData data);
    }
}