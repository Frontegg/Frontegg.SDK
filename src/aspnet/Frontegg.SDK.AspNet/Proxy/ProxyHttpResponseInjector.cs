using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class ProxyHttpResponseInjector : IProxyHttpResponseInjector
    {
        private const int StreamCopyBufferSize = 81920;
        public async Task InjectResponse(HttpContext context, HttpResponseMessage responseMessage)
        {
            var response = context.Response;
            
            response.StatusCode = (int)responseMessage.StatusCode;
            
            foreach (var header in responseMessage.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            if (responseMessage.Content != null)
            {
                foreach (var header in responseMessage.Content.Headers)
                {
                    response.Headers[header.Key] = header.Value.ToArray();
                }
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            if (responseMessage.Content != null)
            {
                using (var responseStream = await responseMessage.Content
                    .ReadAsStreamAsync()
                    .ConfigureAwait(false))
                {
                    await responseStream.CopyToAsync(response.Body, StreamCopyBufferSize, context.RequestAborted).ConfigureAwait(false);
                    
                    if (responseStream.CanWrite)
                    {
                        await responseStream.FlushAsync(context.RequestAborted).ConfigureAwait(false);    
                    }
                }
            }
        }
    }
}