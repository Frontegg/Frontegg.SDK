namespace Frontegg.SDK.Core
{
    internal class InvocationContext : IInvocationContext
    {
        public IRequestContext RequestContext { get; }
        public IResponseContext ResponseContext { get; }

        public InvocationContext(IRequestContext requestContext, IResponseContext responseContext)
        {
            RequestContext = requestContext;
            ResponseContext = responseContext;
        }
    }
}