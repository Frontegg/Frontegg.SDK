using System;
using Owin;

namespace Frontegg.SDK.AspNet.Owin
{
    public static class OwinExtensions
    {
        public static IAppBuilder UseFrontegg(this IAppBuilder builder,
            Action<FronteggOptions> options,
            Action<IAppBuilder> builderAction = null)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            var fronteggOptions = new FronteggOptions();
            options(fronteggOptions);
            var middleware = new FronteggMiddleware(fronteggOptions);
            builder.Map("/frontegg", appBuilder =>
            {
                builderAction?.Invoke(appBuilder);
                appBuilder.Run(middleware.Invoke);
            });
            return builder;
        }
    }
}