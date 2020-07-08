using System;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Frontegg.SDK.AspNet.Authentication
{
    internal class FronteggAuthenticationJob : BackgroundService
    {
        private readonly IFronteggAuthenticator _fronteggAuthenticator;
        private readonly IAuthenticationStateStore _authenticationStateStore;
        private readonly FronteggOptions _options;
        private readonly ILogger<FronteggAuthenticationJob> _logger;
        private int _counter = 0;
        
        public FronteggAuthenticationJob(ILogger<FronteggAuthenticationJob> logger, 
            IFronteggAuthenticator fronteggAuthenticator,
            IAuthenticationStateStore authenticationStateStore, IOptions<FronteggOptions> options)
        {
            _fronteggAuthenticator = fronteggAuthenticator;
            _authenticationStateStore = authenticationStateStore;
            _options = options.Value;
            _logger = logger ?? new NullLogger<FronteggAuthenticationJob>();
        }
 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"Frontegg Authentication is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($"Frontegg Authentication task is stopping."));

            while (!stoppingToken.IsCancellationRequested && _options.IsEnabled)
            {
                var result = await _fronteggAuthenticator.Authenticate(new BasicFronteggCredentials(_options.ClientId, _options.ApiKey), _options)
                    .ConfigureAwait(false);

                _authenticationStateStore.UpdateResult(result);
                
                if (result.IsAuthenticated)
                {
                    _counter = 0;
                    var nextRefresh = (result.ExpiredIn * 1000) * 0.8;
                    await Task.Delay(TimeSpan.FromMilliseconds(nextRefresh), stoppingToken).ConfigureAwait(false);
                }
                else
                {
                    var nextRefresh = GetNextRefreshTimeSpan(_counter);
                    await Task.Delay(nextRefresh, stoppingToken).ConfigureAwait(false);
                    _counter++;
                }
            }
            
            _logger.LogDebug($"Frontegg Authentication is stopping.");
        }

        private static TimeSpan GetNextRefreshTimeSpan(int retriesCount)
        {
            switch (retriesCount)
            {
                case 0:
                    return TimeSpan.FromMilliseconds(200);
                case 1:
                    return TimeSpan.FromMilliseconds(500);
                default:
                    return TimeSpan.FromMilliseconds(1000);
            }
        }
    }
}