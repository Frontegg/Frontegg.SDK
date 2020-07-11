namespace Frontegg.SDK.Client.Authentication
{
    public interface IFronteggCredentials
    {
        string ClientId { get; }
        string ApiKey { get; }
        string Token { get; set; }
    }
}