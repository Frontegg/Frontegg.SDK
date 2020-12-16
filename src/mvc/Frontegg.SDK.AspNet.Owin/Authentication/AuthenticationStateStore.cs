using System;
using System.Threading;
using System.Threading.Tasks;

namespace Frontegg.SDK.AspNet.Owin.Authentication
{
    internal class AuthenticationStateStore : IAuthenticationStateStore
    {
        private readonly FronteggOptions _options;
        private readonly IAuthenticator _authenticator;
        private bool _isInitialized = false;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private FronteggAuthenticationState _authenticationState = InitialState;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;
        private readonly Task _updateProcess;
        private bool _isDisposed = false;

        public AuthenticationStateStore(FronteggOptions options)
            : this(options, new Authenticator(options))
        { }
        
        internal AuthenticationStateStore(FronteggOptions options, IAuthenticator authenticator)
        {
            _options = options;
            _authenticator = authenticator;
            _cancellationToken = _cancellationTokenSource.Token;
            _updateProcess = new Task(async () => await TokenUpdateProcess().ConfigureAwait(false), _cancellationToken, TaskCreationOptions.RunContinuationsAsynchronously);
        }
        
        private static readonly FronteggAuthenticationState InitialState =
            FronteggAuthenticationState.FailedResult("Client has not been authenticated yet.");

        public async Task<FronteggAuthenticationState> GetLastState()
        {
            await HandleFirstTime().ConfigureAwait(false);
            
            _lock.EnterReadLock();
            try
            {
                return FronteggAuthenticationState.CloneResult(_authenticationState);
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
                            throw new FailedAuthorisationsException($"Failed to authenticate clientId: {_options.ClientId}");
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
        
        private void UpdateState(FronteggAuthenticationState authenticationState)
        {
            _lock.EnterWriteLock();
            try
            {
                _authenticationState = FronteggAuthenticationState.CloneResult(authenticationState);

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

        private async Task<FronteggAuthenticationState> GetAuthenticationResult()
        {
            var result = await _authenticator.Authenticate(_options.ClientId, _options.ApiKey)
                .ConfigureAwait(false);
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