using System;
using System.Threading.Tasks;
using Xunit;

namespace Frontegg.SDK.Client.Tests
{
    public class UnitTest1
    {
        //[Fact]
        public async Task Test1()
        {
            var client = new FronteggClient(
                new FronteggCredentials("", ""),
                o =>
                {
                    o.FronteggUrl = new Uri("https://dev-api.frontegg.com");
                });
            
            var t = new { Ip = "161.185.160.93", approvers = new string[] {"aviad", "sagi", "royi" },moshe= new { firstName ="firstName", lastName= "lastName"} ,user= "Albert Cohen", action="Accessed", resource="Dashboard", severity="Info"  };

            
            await client.Audits.SendAudit("my-tenant-id", t);

        }
    }
}