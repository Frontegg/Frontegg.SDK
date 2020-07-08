using System;
using System.Threading.Tasks;
using Frontegg.SDK.Core.Authentication;

namespace Frontegg.SDK.Core
{
    public abstract class FronteggClient : IDisposable
    {
        private bool _isDisposed;
        private readonly IActionInvoker _actionInvoker;
        private IFronteggCredentials FronteggCredentials { get; }
        private ClientConfig Config { get; }

        protected IServiceMetadata Metadata { get; set; } = new ServiceMetadata();

        protected FronteggClient(IFronteggCredentials fronteggCredentials, ClientConfig config)
        {
            FronteggCredentials = fronteggCredentials;
            Config = config;
        }

        internal FronteggClient()
        {
            _actionInvoker = new ActionInvoker();
        }
        
        protected TResponse Invoke<TResponse>(FronteggWebRequest request, InvokeOptions options) where TResponse : FronteggWebResponse
        {
            ThrowIfDisposed();

            var invocationContext = new InvocationContext(new RequestContext
            {
                ClientConfig = Config,
                OriginalRequest = request,
                Options = options,
                ServiceMetadata = Metadata
            }, new ResponseContext());
            
            var responseContext = _actionInvoker.InvokeSync(invocationContext);
            
            return (TResponse)responseContext.Response;
        }

        protected Task<TResponse> InvokeAsync<TResponse>() where TResponse : FronteggWebResponse
        {
            ThrowIfDisposed();
            
            
            return null;
        }
        
        private void ThrowIfDisposed()
        {
           if(_isDisposed)
               throw new ObjectDisposedException(GetType().FullName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _isDisposed = true;
            }
        }
    }

    public interface IServiceMetadata
    {
        public string ServiceName { get; }
    }
    
    class ServiceMetadata : IServiceMetadata
    {
        public string ServiceName => "";
    }

    public class InvokeOptions
    {
    }

    public class FronteggWebRequest : IFronteggWebRequest
    {
    }

    public interface IFronteggWebRequest
    {
    }

    internal class ActionInvoker : IActionInvoker
    {
        public IResponseContext InvokeSync(IInvocationContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseContext> InvokeAsync(IInvocationContext context)
        {
            throw new NotImplementedException();
        }
    }

    internal class ActionInvokerBuilder
    {
        
    }

    internal class ActionHandlerHolder
    {
        private readonly IActionInvoker _actionInvoker;

        public Type Type { get; }

        public ActionHandlerHolder(IActionInvoker actionInvoker)
        {
            _actionInvoker = actionInvoker;
            Type = actionInvoker.GetType();
        }
        
        public ActionHandlerHolder Inner { get; set; }
        public ActionHandlerHolder Outer { get; set; }
    }
    
    public interface IActionHandler 
    {
        
    }
    
    public delegate void X(IInvocationContext context);
}