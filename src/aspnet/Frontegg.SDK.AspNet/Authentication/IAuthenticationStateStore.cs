namespace Frontegg.SDK.AspNet.Authentication
{
    internal interface IAuthenticationStateStore
    {
        void UpdateResult(FronteggAuthenticationResult result);
        FronteggAuthenticationResult GetLatestState();
    }
}