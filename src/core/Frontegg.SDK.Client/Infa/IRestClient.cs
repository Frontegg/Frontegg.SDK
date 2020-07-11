using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Infa
{
    internal interface IRestClient
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage, CancellationToken cancellationToken);
    }
}