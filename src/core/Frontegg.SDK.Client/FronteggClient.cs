using System;
using System.Net.Http;
using Frontegg.SDK.Client.Audits;
using Frontegg.SDK.Client.Authentication;
using Frontegg.SDK.Client.Net;

namespace Frontegg.SDK.Client
{
    public class FronteggClient : IFronteggClient
    {
        public IFronteggCredentials Credentials { get; }

        public Uri FronteggUrl => Options.FronteggUrl;

        private FronteggClientOptions Options { get; set; }

        public IAuditsClient Audits { get; }

        public FronteggClient(IFronteggCredentials credentials,
            Action<FronteggClientOptions> setupOptions = null)
        {
            Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            Options = SetupOptions(setupOptions);
            var authenticator = new Authenticator(Options.FronteggUrl);
            var authenticationStateStore = new AuthenticationStateStore(authenticator, Credentials);
            var restClient = new RestClient(authenticationStateStore, Options.DefaultTimeout,
                () => new HttpClientHandler()); //should be replaced
            Audits = new AuditsClient(Options, restClient);
        }

        private static FronteggClientOptions SetupOptions(Action<FronteggClientOptions> action)
        {
            var options = new FronteggClientOptions();
            action?.Invoke(options);
            ValidateOptions(options);
            return options;
        }

        private static void ValidateOptions(FronteggClientOptions options)
        {
            var validator = new FronteggOptionsValidator();
            validator.Validate(options);
        }
    }
}