using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class HandleProxyRequestWrapper: IFronteggProxyInfoExtractor
    {
        private readonly HandleProxyRequest _wrapper;

        public HandleProxyRequestWrapper(HandleProxyRequest wrapper)
        {
            _wrapper = wrapper;
        }

        public Task<FronteggTenantInfo> Extract(HttpRequest request)
        {
            return _wrapper(request);
        }
    }
}