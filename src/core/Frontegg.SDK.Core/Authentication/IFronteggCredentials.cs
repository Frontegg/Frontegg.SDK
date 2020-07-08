namespace Frontegg.SDK.Core.Authentication
{
    public interface IFronteggCredentials
    {
        string ClientId { get; }
        string ApiKey { get; }
    }
}