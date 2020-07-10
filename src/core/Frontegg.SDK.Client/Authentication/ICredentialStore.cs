namespace Frontegg.SDK.Client.Authentication
{
    internal interface ICredentialStore
    {
        IFronteggCredentials GetCredentials();
    }
}