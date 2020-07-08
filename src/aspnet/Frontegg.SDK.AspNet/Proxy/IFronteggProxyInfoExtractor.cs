using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    public interface IFronteggProxyInfoExtractor
    {
        Task<FronteggTenantInfo> Extract(HttpRequest request);
    }
}