using System;

namespace Frontegg.SDK.Client
{
    internal class FronteggOptionsValidator
    {
        public void Validate(FronteggClientOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            if(options.DefaultTimeout < TimeSpan.Zero)
                throw new InvalidOperationException($"{nameof(options.DefaultTimeout)} value can not be lower than 0");

            if (options.JsonSerializer == null)
                throw new InvalidOperationException($"{nameof(options.JsonSerializer)} can not be null");

            if (options.FronteggUrl == null)
                throw new InvalidOperationException($"{nameof(options.FronteggUrl)} can not be null");
        }
    }
}