using System;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Authentication
{
    internal interface IAuthenticationStateStore: IDisposable
    {
        Task<FronteggAuthenticationState> GetLatestState();
    }
}