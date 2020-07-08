namespace Frontegg.SDK.AspNet.Authentication
{
    internal class FronteggSuccessfulAuthenticationResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}