using System;
using Frontegg.SDK.Client.Authentication;
using Frontegg.SDK.Client.Net;

namespace Frontegg.SDK.Client
{
    internal class FronteggClient : IFronteggClient
    {
        private readonly IAuthenticator _authenticator;
        public FronteggCredentials Credentials { get; }
        
        private UrlBuilder _urlBuilder;
        
        public IAuditsClient Audits { get; }

        public FronteggClient(FronteggCredentials credentials) 
            : this(credentials, null)
        {
        }

        internal FronteggClient(FronteggCredentials credentials,
            IAuthenticator authenticator = null)
        {
            _authenticator = authenticator ?? new Authenticator();
            Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }
    }
}