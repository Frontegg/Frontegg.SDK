using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    internal class ProxyHttpResponseInjector : IProxyHttpResponseInjector
    {
        private const int StreamCopyBufferSize = 81920;
        public async Task InjectResponse(IOwinContext context, HttpResponseMessage responseMessage)
        {
            var response = context.Response;
            
            response.StatusCode = (int)responseMessage.StatusCode;
            
            foreach (var header in responseMessage.Headers)
            {
                // check this 
                response.Headers[header.Key] = string.Join(",",header.Value);
            }

            if (responseMessage.Content != null)
            {
                foreach (var header in responseMessage.Content.Headers)
                {
                    response.Headers[header.Key] = string.Join(",",header.Value);
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
                    await responseStream.CopyToAsync(response.Body, StreamCopyBufferSize, context.Request.CallCancelled).ConfigureAwait(false);
                    
                    if (responseStream.CanWrite)
                    {
                        await responseStream.FlushAsync(context.Request.CallCancelled).ConfigureAwait(false);    
                    }
                }
            }
        }
    }
}