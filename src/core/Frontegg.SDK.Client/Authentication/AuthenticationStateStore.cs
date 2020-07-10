using System;
using System.Threading;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Authentication
{
    internal class AuthenticationStateStore : IAuthenticationStateStore
    {
        private static readonly FronteggAuthenticationResult InitialState =
            FronteggAuthenticationResult.FailedResult("Client has not been authenticated yet.");
        private readonly IAuthenticator _authenticator;
        private readonly IFronteggCredentials _credentials;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private FronteggAuthenticationResult _authenticationState = InitialState;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;
        private readonly Task _updateProcess;
        private bool _isDisposed = false;
            
        public AuthenticationStateStore(IAuthenticator authenticator,
            IFronteggCredentials credentials)
        {
            _authenticator = authenticator;
            _credentials = credentials;
            _cancellationToken = _cancellationTokenSource.Token;
            _updateProcess = new Task(async () => await TokenUpdateProcess().ConfigureAwait(false), _cancellationToken, TaskCreationOptions.RunContinuationsAsynchronously) ;
        }
        
        public async Task<FronteggAuthenticationResult> GetLatestState()
        {
            await HandleFirstTime().ConfigureAwait(false);
            
            _lock.EnterReadLock();
            try
            {
                return FronteggAuthenticationResult.CloneResult(_authenticationState);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private async Task HandleFirstTime()
        {
            if (!_isInitialized)
            {
                await _semaphore.WaitAsync(_cancellationToken).ConfigureAwait(false);
                try
                {
                    if (!_isInitialized)
                    {
                        var result = InitialState;
                        
                        for (var i = 0; i < 3; i++)
                        {
                            result = await GetAuthenticationResult().ConfigureAwait(false);
                            if (result.IsAuthenticated)
                            {
                                UpdateState(result);
                                _updateProcess.Start();
                                _isInitialized = true;
                                return;
                            }
                            var nextRefresh = GetNextRefreshTimeSpan(i);
                            await Task.Delay(nextRefresh, _cancellationToken).ConfigureAwait(false);
                        }

                        if (!result.IsAuthenticated)
                        {
                            throw new FailedAuthorisationsException($"Failed to authenticate clientId: {_credentials.ClientId}");
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
        
        private void UpdateState(FronteggAuthenticationResult authenticationResult)
        {
            _lock.EnterWriteLock();
            try
            {
                _authenticationState = FronteggAuthenticationResult.CloneResult(authenticationResult);

            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        
        private async Task TokenUpdateProcess()
        {
            var counter = 0;
            
            while (!_cancellationToken.IsCancellationRequested)
            {
                var result = await GetAuthenticationResult().ConfigureAwait(false);

                if (result.IsAuthenticated)
                {
                    UpdateState(result);
                    counter = 0;
                    var nextRefresh = (result.ExpiredIn * 1000) * 0.8;
                    await Task.Delay(TimeSpan.FromMilliseconds(nextRefresh), _cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    var nextRefresh = GetNextRefreshTimeSpan(counter);
                    await Task.Delay(nextRefresh, _cancellationToken).ConfigureAwait(false);
                    counter++;
                }
            }
        }

        private async Task<FronteggAuthenticationResult> GetAuthenticationResult()
        {
            var result = await _authenticator.Authenticate(_credentials).ConfigureAwait(false);
            return result;
        }
        
        private static TimeSpan GetNextRefreshTimeSpan(int retriesCount)
        {
            switch (retriesCount)
            {
                case 0:
                    return TimeSpan.FromMilliseconds(200);
                case 1:
                    return TimeSpan.FromMilliseconds(500);
                case 2:
                    return TimeSpan.FromMilliseconds(1000);
                default:
                    return TimeSpan.FromMilliseconds(2000);
            }
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
                _cancellationTokenSource?.Cancel();
               _updateProcess?.Dispose();
            }

            _isDisposed = true;
        }
    }
}