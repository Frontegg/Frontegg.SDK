namespace Frontegg.SDK.Client
{
    public interface IFronteggClient
    {
        IAuditsClient Audits { get; }
    }
}