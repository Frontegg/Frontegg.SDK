using System;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var credentials = new FronteggCredentials("", "");
            
            var client = new FronteggClient(credentials);

            await client.Audits.SendAudit("", "{ }");
        }
    }
    
    
}