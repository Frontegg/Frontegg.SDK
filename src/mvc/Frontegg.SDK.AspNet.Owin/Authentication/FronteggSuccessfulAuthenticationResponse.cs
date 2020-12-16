namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    internal class FronteggSuccessfulAuthenticationResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}