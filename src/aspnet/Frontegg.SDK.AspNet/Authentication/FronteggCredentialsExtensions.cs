namespace Frontegg.SDK.AspNet.Authentication
{
    public static class FronteggCredentialsExtensions
    {
        public static bool HasToken(this IFronteggCredentials target)
        {
            return string.IsNullOrWhiteSpace(target.Token);
        }
    }
}