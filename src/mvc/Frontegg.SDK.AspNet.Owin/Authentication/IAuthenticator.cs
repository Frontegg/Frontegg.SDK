using System.Threading.Tasks;

namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    internal interface IAuthenticator
    {
        Task<FronteggAuthenticationState> Authenticate(string clientId, string apiKey);
    }
}