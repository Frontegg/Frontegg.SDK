namespace Frontegg.SDK.Core
{
    public interface IRequestContext
    {
        ClientConfig ClientConfig { get; set; }
        FronteggWebRequest OriginalRequest { get; set; }
    }

    internal class RequestContext : IRequestContext
    {
        public ClientConfig ClientConfig { get; set; }
        public FronteggWebRequest OriginalRequest { get; set; }
        public InvokeOptions Options { get; set; }
        public IServiceMetadata ServiceMetadata { get; set; }
    }
}