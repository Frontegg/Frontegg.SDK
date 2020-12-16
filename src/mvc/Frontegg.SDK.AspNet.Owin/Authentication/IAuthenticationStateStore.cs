using System.Threading.Tasks;

namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    internal interface IAuthenticationStateStore
    {
        Task<FronteggAuthenticationState> GetLastState();
    }
}