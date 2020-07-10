using System;
using System.Threading.Tasks;
using Xunit;

namespace Frontegg.SDK.Client.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var client = new Frontegg.SDK.Client.FronteggClient(new FronteggCredentials("8cd7350d-69cd-4ff1-a1d9-4eff30687979", "989c8743-8bfb-448e-bd30-7b148c6cc20a"),
                "https://dev-api.frontegg.com");

            await client.Audits.SendAudits("my-tenant-id", new[] {"this is an audit"});

        }
    }
}