using System.Threading.Tasks;

namespace Frontegg.SDK.AspNet.Authentication
{
    internal interface IFronteggAuthenticator
    {
        Task<FronteggAuthenticationResult> Authenticate(IFronteggCredentials fronteggCredentials,
            FronteggOptions options);
    }
}