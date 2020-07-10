using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Authentication
{
    internal interface IAuthenticator
    {
        Task<FronteggAuthenticationResult> Authenticate(IFronteggCredentials fronteggCredentials);
    }
}