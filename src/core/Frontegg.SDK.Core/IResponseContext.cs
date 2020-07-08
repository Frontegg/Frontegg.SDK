namespace Frontegg.SDK.Core
{
    public interface IResponseContext
    {
        FronteggWebResponse Response { get; }
    }

    class ResponseContext : IResponseContext
    {
        public FronteggWebResponse Response { get; }
        
    }
}