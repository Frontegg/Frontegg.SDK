using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal interface IFronteggProxyResponseCreator
    {
        Task<HttpResponseMessage> GenerateProxyResponse(HttpContext context);
    }
}