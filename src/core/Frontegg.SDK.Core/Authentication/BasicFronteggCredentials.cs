using System;

namespace Frontegg.SDK.Core.Authentication
{
    public class BasicFronteggCredentials : IFronteggCredentials
    {
        public string ClientId { get; }
        public string ApiKey { get; }

        public BasicFronteggCredentials(string clientId, string apiKey)
        {
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }
    }
}