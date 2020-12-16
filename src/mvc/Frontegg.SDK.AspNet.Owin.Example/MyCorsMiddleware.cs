using System.Threading.Tasks;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Example
{
    public class MyCorsMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public MyCorsMiddleware(OwinMiddleware next)
            : base(next)
        {
            _next = next;
        }

        public override async Task Invoke(IOwinContext x)
        {
            x.Response.Headers.Add("Access-Control-Allow-Origin", new[] {x.Request.Headers["Origin"] ?? "localhost"});
            x.Response.Headers.Add("Access-Control-Allow-Credentials", new[] {"true"});
            x.Response.Headers.Add("Access-Control-Allow-Methods",
                new[] {"POST", "GET", "PUT", "PATCH", "OPTIONS", "DELETE"});
            x.Response.Headers.Add("Access-Control-Max-Age", new string[] {"3600"});
            x.Response.Headers.Add("Access-Control-Allow-Headers",
                new[] {"Content-Type", "Accept", "x-frontegg-source", "Authorization"});
            await _next.Invoke(x);
        }
    }
}