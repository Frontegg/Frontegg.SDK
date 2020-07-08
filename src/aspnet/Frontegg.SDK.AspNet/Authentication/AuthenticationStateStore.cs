using System.Threading;

namespace Frontegg.SDK.AspNet.Authentication
{
    internal class AuthenticationStateStore : IAuthenticationStateStore
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private FronteggAuthenticationResult _authenticationResult;
        
        public void UpdateResult(FronteggAuthenticationResult result)
        {
            _lock.EnterWriteLock();
            try
            {
                _authenticationResult = result;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public FronteggAuthenticationResult GetLatestState()
        {
            _lock.EnterReadLock();
            try
            {
                return FronteggAuthenticationResult.CloneResult(_authenticationResult);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}