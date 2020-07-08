using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal interface IProxyHttpResponseInjector
    {
        Task InjectResponse(HttpContext context, HttpResponseMessage responseMessage);
    }
}