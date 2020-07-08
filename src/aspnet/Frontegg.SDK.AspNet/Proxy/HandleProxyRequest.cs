using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    public delegate Task<FronteggTenantInfo> HandleProxyRequest(HttpRequest request);
}