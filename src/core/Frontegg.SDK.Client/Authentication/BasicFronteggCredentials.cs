namespace Frontegg.SDK.Client.Authentication
{
    internal class BasicFronteggCredentials : IFronteggCredentials
    {
        public string ClientId { get; }
        public string ApiKey { get; }

        string IFronteggCredentials.Token { get; set; }

        public BasicFronteggCredentials(string clientId, string apiKey)
        {
            ClientId = clientId;
            ApiKey = apiKey;
        }
    }
}