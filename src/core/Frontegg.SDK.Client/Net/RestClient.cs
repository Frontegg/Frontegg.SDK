using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.Client.Authentication;

namespace Frontegg.SDK.Client.Net
{
    public class RestClient : Infa.IRestClient
    {
        private readonly Func<HttpMessageHandler> _handlerCreator;

        public RestClient(Func<HttpMessageHandler> handlerCreator)
        {
            _handlerCreator = handlerCreator;
        }
        
        // change HttpRequestMessage to something ours
        public async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            // TimeSpan.FromSeconds(30) change this to configuration
            var cancellationTokenForRequest = GetCancellationTokenForRequest(TimeSpan.FromSeconds(30) , cancellationToken);

            using (requestMessage)
            {
                var httpMessageHandler = _handlerCreator();
                using (var httpClient = new HttpClient(new HttpClientHandlerWrapper
                    {InnerHandler = httpMessageHandler}))
                {
                    var response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead,
                        cancellationTokenForRequest).ConfigureAwait(false);

                    return response;
                }
            }
        }
        
        static CancellationToken GetCancellationTokenForRequest(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var cancellationTokenForRequest = cancellationToken;

            if (timeout == TimeSpan.Zero) 
                return cancellationTokenForRequest;
            
            var timeoutCancellation = new CancellationTokenSource(timeout);
            var unifiedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellation.Token);

            cancellationTokenForRequest = unifiedCancellationToken.Token;
            return cancellationTokenForRequest;
        }
    }
}