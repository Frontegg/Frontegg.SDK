namespace Frontegg.SDK.Client
{
    internal static class Constants
    {
        public const string FronteggApiUrl = "https://api.frontegg.com/";
        public const string AuthorizationPath = "/auth/vendor";
        public const string AuditsUrl = "/audits";
        public const string MetadataUrl = "/metadata";
        public const string AccessTokenHeaderKey = "x-access-token";
        public const string TenantIdHeaderKey = "frontegg-tenant-id";
    }
}