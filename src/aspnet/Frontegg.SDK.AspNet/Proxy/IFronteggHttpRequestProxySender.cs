using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal interface IFronteggHttpRequestProxySender
    {
        Task<HttpResponseMessage> ProxyHttpRequest(HttpRequestMessage httpRequestMessage, HttpContext context);
    }
}