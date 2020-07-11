namespace Frontegg.SDK.Client.Authentication
{
    internal class FronteggSuccessfulAuthenticationResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}