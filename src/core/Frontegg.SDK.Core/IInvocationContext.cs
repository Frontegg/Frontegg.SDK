namespace Frontegg.SDK.Core
{
    public interface IInvocationContext
    {
        IRequestContext RequestContext { get; }
        IResponseContext ResponseContext { get; }
    }
}