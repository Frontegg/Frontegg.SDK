using System.Net.Http;
using Frontegg.SDK.AspNet.Owin.Http;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    internal interface IFronteggHttProxyRequestCreator
    {
        HttpRequestMessage CreateProxyRequest(IOwinContext context, ProxyHttpRequestData data);
    }
}