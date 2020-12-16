using System;
using System.Threading;
using System.Web.Hosting;
using Frontegg.SDK.AspNet.Owin.Example;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("Configuration", typeof(Startup))]

namespace Frontegg.SDK.AspNet.Owin.Example
{
    public class Startup :IRegisteredObject
    {
        public Startup()
        {
            HostingEnvironment.RegisterObject(this);
        }
        
        public void Configuration(IAppBuilder app)
        {
            app.UseFrontegg(options =>
            {
                options.ApiKey = "api-key";
                options.ClientId = "client-id";
                options.FronteggProxyInfoExtractor = new MyTenantExtractor();
            }, builderAction =>
            {
                builderAction.Use<MyCorsMiddleware>();
            });
        }
        
        public void Stop(bool immediate)
        {
            Thread.Sleep(TimeSpan.FromSeconds(30));
            HostingEnvironment.UnregisterObject(this);
        }
    }
}