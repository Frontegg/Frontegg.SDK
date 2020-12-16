namespace Frontegg.SDK.AspNet.Owin
{
    internal static class Frontegg
    {
        public const string FronteggBaseUrl = "https://api.frontegg.com";
        public static readonly string FronteggAuthenticationUrl = "auth/vendor";
        public const string AccessTokenHeaderKey = "x-access-token";
        public const string TenantIdHeaderKey = "frontegg-tenant-id";
        public const string UserIdHeaderKey = "frontegg-user-id";
    }
}