using System;

namespace Frontegg.SDK.AspNet.Owin.Http
{
    internal class ProxyHttpRequestData
    {
        public Uri FronteggUrl { get; set; }
        public string Token { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }
    }
}