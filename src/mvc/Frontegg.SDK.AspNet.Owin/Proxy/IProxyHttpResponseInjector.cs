using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    internal interface IProxyHttpResponseInjector
    {
        Task InjectResponse(IOwinContext context, HttpResponseMessage responseMessage);
    }
}