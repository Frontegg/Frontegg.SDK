using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    internal class FronteggHttpRequestProxySender : IFronteggHttpRequestProxySender
    {
        private readonly FronteggOptions _fronteggOptions;

        internal FronteggHttpRequestProxySender(FronteggOptions fronteggOptions)
        {
            _fronteggOptions = fronteggOptions;
        }

        public async Task<HttpResponseMessage> ProxyHttpRequest(HttpRequestMessage httpRequestMessage,
            IOwinContext context)
        {
            try
            {
                var httpClient =
                    new HttpClient(new HttpClientHandler() {AllowAutoRedirect = false, UseCookies = false})
                    {
                        BaseAddress = new Uri(_fronteggOptions.Url)
                    };

                return await httpClient
                    .SendAsync(
                        httpRequestMessage,
                        HttpCompletionOption.ResponseHeadersRead,
                        context.Request.CallCancelled)
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
            catch (HttpRequestException ex) when (ex.InnerException is IOException ||
                                                  ex.InnerException is SocketException)
            {
                // log this
                // Happens when server is not reachable
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}