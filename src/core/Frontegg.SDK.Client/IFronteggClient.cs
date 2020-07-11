using Frontegg.SDK.Client.Audits;

namespace Frontegg.SDK.Client
{
    public interface IFronteggClient
    {
        IAuditsClient Audits { get; }
    }
}