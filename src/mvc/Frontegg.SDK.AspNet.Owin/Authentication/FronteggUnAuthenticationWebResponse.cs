namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    internal class FronteggUnAuthenticationWebResponse
    {
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
    }
}