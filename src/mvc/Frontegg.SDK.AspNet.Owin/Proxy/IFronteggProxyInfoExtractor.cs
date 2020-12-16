using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    public interface IFronteggProxyInfoExtractor
    {
        FronteggTenantInfo Extract(IOwinRequest request);
    }
}