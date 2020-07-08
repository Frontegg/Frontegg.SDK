namespace Frontegg.SDK.AspNet
{
    internal static class Frontegg
    {
        public const string FronteggBaseUrl = "https://api.frontegg.com";
        public static readonly string FronteggAuthenticationUrl = $"{FronteggBaseUrl}/auth/vendor";
        public const string ClientName = "FrontggProxyClient";
        public const string AccessTokenHeaderKey = "x-access-token";
        public const string TenantIdHeaderKey = "frontegg-tenant-id";
        public const string UserIdHeaderKey = "frontegg-user-id";
    }
}