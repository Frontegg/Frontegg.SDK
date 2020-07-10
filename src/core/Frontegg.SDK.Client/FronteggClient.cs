using System;
using System.Net.Http;
using Frontegg.SDK.Client.Authentication;
using Frontegg.SDK.Client.Infa;
using Frontegg.SDK.Client.Net;

namespace Frontegg.SDK.Client
{
    public class FronteggClient : IFronteggClient
    {
        internal IClientContext Context { get; set; }
        
        public IFronteggCredentials Credentials { get; }

        public Uri FronteggUrl { get; }

        public IJsonSerializer JsonSerializer
        {
            get => Context.JsonSerializer;
            set => Context.JsonSerializer = value;
        }

        public IAuditsClient Audits { get; }

        public FronteggClient(IFronteggCredentials credentials)
            :this(credentials, Constants.FronteggApiUrl)
        { }
        
        public FronteggClient(IFronteggCredentials credentials, string fronteggUrl) 
            : this(credentials, fronteggUrl, null)
        { }

        internal FronteggClient(IFronteggCredentials credentials,
            string fronteggUrl,
            IAuthenticator authenticator = null,
            IRestClient restClient = null,
            IAuthenticationStateStore authenticationStateStore = null)
        {
            Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            FronteggUrl = new Uri(fronteggUrl);
            var urlBuilder = new UrlBuilder(fronteggUrl)
                .WithPath(Constants.AuthorizationPath);
            authenticator = authenticator ?? new Authenticator(urlBuilder.ToString());
            authenticationStateStore = authenticationStateStore ?? new AuthenticationStateStore(authenticator, Credentials);
            restClient = restClient ?? new RestClient(authenticationStateStore, () => new HttpClientHandler()); //should be replaced
            
            Context = new ClientContext
            {
                JsonSerializer = new InnerJsonSerializer(o => o.PropertyNameCaseInsensitive = true),
                RestClient = restClient,
                FronteggApiUrl = FronteggUrl
            };
            
            Audits = new AuditsClient(fronteggUrl, Context);
        }
    }
}