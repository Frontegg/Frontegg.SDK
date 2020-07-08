namespace Frontegg.SDK.AspNet
{
    public class FronteggOptions
    {
        public string Url { get; set; } = Frontegg.FronteggBaseUrl;
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public bool ThrowOnMissingConfiguration { get; set; } = false;
        internal string AuthenticationRelativePath { get; set; } = Frontegg.FronteggAuthenticationUrl;
        internal bool IsEnabled { get; set; } = true;
    }
}