using Frontegg.SDK.AspNet.Owin.Proxy;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Example
{
    public class MyTenantExtractor : IFronteggProxyInfoExtractor
    {
        public FronteggTenantInfo Extract(IOwinRequest request)
        {
            return new FronteggTenantInfo()
            {
                TenantId = "my-tenant-id",
                UserId = "userId"
            };
        }
    }
}