using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frontegg.SDK.AspNet
{
    internal class FronteggConfigurator : IConfigureOptions<FronteggOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ICustomFronteggOptionsConfigurationProvider _fronteggOptionsConfigurationProvider;

        public FronteggConfigurator(IServiceScopeFactory serviceScopeFactory, ICustomFronteggOptionsConfigurationProvider fronteggOptionsConfigurationProvider)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _fronteggOptionsConfigurationProvider = fronteggOptionsConfigurationProvider;
        }

        public void Configure(FronteggOptions options)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;

                var configuration = provider.GetService<IConfiguration>();
                configuration.GetSection("Frontegg")
                    .Bind(options);
            
                _fronteggOptionsConfigurationProvider
                    .FronteggConfiguration?.Invoke(options);
            }
        }
    }
}