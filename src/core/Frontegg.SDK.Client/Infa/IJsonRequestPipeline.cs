using System.Net.Http;
using System.Threading.Tasks;

namespace Frontegg.SDK.Client.Infa
{
    internal interface IJsonRequestPipeline
    {
        Task SerializeRequest(HttpRequestMessage request, object obj);
        Task<T> DeserializeResponse<T>(HttpResponseMessage response);
    }
}