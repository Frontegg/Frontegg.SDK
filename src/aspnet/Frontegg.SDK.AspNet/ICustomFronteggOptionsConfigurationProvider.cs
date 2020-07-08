using System;

namespace Frontegg.SDK.AspNet
{
    internal interface ICustomFronteggOptionsConfigurationProvider
    {
        Action<FronteggOptions> FronteggConfiguration { get; set; }
    }
}