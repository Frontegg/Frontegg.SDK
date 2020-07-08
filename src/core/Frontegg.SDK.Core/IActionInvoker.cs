using System.Threading.Tasks;

namespace Frontegg.SDK.Core
{
    internal interface IActionInvoker
    {
        IResponseContext InvokeSync(IInvocationContext context);
        Task<IResponseContext> InvokeAsync(IInvocationContext context);
    }
}