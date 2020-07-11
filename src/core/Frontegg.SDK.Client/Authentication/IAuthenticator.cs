using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Authentication
{
    internal interface IAuthenticator
    {
        Task<FronteggAuthenticationState> Authenticate(IFronteggCredentials fronteggCredentials);
    }
}