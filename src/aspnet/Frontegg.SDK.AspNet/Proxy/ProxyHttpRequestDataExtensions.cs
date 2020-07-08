using System;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal static class ProxyHttpRequestDataExtensions
    {
        public static void Validate(this ProxyHttpRequestData target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            var validator = new ProxyHttpRequestDataValidator();
            
            validator.Validate(target);
        }
    }
}