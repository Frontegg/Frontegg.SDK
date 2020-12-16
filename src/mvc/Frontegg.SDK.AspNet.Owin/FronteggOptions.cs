using Frontegg.SDK.AspNet.Owin.Proxy;

namespace Frontegg.SDK.AspNet.Owin
{
    public class FronteggOptions
    {
        public string Url { get; set; } = Frontegg.FronteggBaseUrl;
        public string ClientId { get; set; }
        public string ApiKey { get; set; }

        public IFronteggProxyInfoExtractor FronteggProxyInfoExtractor { get; set; }

        internal string AuthenticationPath { get; set; } = Frontegg.FronteggAuthenticationUrl;
        internal bool IsEnabled { get; set; } = true;
    }
}