using System;
using System.Net.Http;

namespace Frontegg.SDK.Client.Net
{
    internal static class HttpRequestMessageExtensions
    {
        public static void AddAuthorizationHeader(this HttpRequestMessage target, string tokenId)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            target.Headers.Add(Constants.AccessTokenHeaderKey, tokenId);
        }

        public static void AddTenantIdHeader(this HttpRequestMessage target, string tenantId)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            target.Headers.Add(Constants.TenantIdHeaderKey, tenantId);
        }
    }
}