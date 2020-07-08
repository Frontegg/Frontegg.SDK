using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Frontegg.SDK.AspNet.Proxy
{
    internal class FronteggHttpRequestProxySender : IFronteggHttpRequestProxySender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FronteggHttpRequestProxySender> _logger;

        public FronteggHttpRequestProxySender(IHttpClientFactory httpClientFactory, ILogger<FronteggHttpRequestProxySender> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        
        public async Task<HttpResponseMessage> ProxyHttpRequest(HttpRequestMessage httpRequestMessage, HttpContext context)
        {
            var httpClient = _httpClientFactory.CreateClient(Frontegg.ClientName);
            
            try
            {
                return await httpClient
                    .SendAsync(
                        httpRequestMessage,
                        HttpCompletionOption.ResponseHeadersRead,
                        context.RequestAborted)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is IOException)
            {
                return new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
            }
            catch (OperationCanceledException)
            {
                // Happens when Timeout is low and upstream host is not reachable.
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
            catch (HttpRequestException ex) when (ex.InnerException is IOException || ex.InnerException is SocketException)
            {
                // log this
                // Happens when server is not reachable
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}