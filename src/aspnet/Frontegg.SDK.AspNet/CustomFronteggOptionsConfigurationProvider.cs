using System;

namespace Frontegg.SDK.AspNet
{
    internal class CustomFronteggOptionsConfigurationProvider : ICustomFronteggOptionsConfigurationProvider
    {
        public Action<FronteggOptions> FronteggConfiguration { get; set; }
        
        public CustomFronteggOptionsConfigurationProvider(Action<FronteggOptions> fronteggConfiguration)
        {
            FronteggConfiguration = fronteggConfiguration;
        }
    }
}