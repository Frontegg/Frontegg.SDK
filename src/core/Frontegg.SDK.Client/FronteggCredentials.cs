namespace Frontegg.SDK.Client
{
    public class FronteggCredentials
    {
        public string ClientId { get; set; }
        public string AccessKey { get; set; }

        public FronteggCredentials(string clientId, string accessKey)
        {
            ClientId = clientId;
            AccessKey = accessKey;
        }
    }
}