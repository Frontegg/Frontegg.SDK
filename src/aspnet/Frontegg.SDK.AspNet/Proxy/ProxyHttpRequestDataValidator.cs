using System;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class ProxyHttpRequestDataValidator
    {
        public void Validate(ProxyHttpRequestData requestData)
        {
            if(requestData.UserId == null) throw new ArgumentNullException(nameof(ProxyHttpRequestData.UserId));
            if(requestData.TenantId == null) throw new ArgumentNullException(nameof(ProxyHttpRequestData.TenantId));
            if(requestData.Token == null) throw new ArgumentNullException(nameof(ProxyHttpRequestData.Token));
        }
    }
}