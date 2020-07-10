using Frontegg.SDK.Client.Authentication;

namespace Frontegg.SDK.Client
{
    public class FronteggCredentials: IFronteggCredentials
    {
        public string ClientId { get; set; }
        public string ApiKey { get; }
        public string Token { get; set; }

        public FronteggCredentials(string clientId, string apiKey)
        {
            ClientId = clientId;
            ApiKey = apiKey;
        }
    }
}