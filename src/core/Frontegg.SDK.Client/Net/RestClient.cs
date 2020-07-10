using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frontegg.SDK.Client.Authentication;
using Frontegg.SDK.Client.Infa;

namespace Frontegg.SDK.Client.Net
{
    internal class RestClient : IRestClient
    {
        private readonly IAuthenticationStateStore _authenticationStateStore;
        private readonly Func<HttpMessageHandler> _handlerCreator;

        public RestClient(IAuthenticationStateStore authenticationStateStore,
            Func<HttpMessageHandler> handlerCreator)
        {
            _authenticationStateStore = authenticationStateStore;
            _handlerCreator = handlerCreator;
        }
        
        // change HttpRequestMessage to something ours
        public async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            var latestState = await _authenticationStateStore.GetLatestState()
                .ConfigureAwait(false);

            if (!latestState.IsAuthenticated)
            {
                throw new FronteggHttpException(HttpStatusCode.Forbidden, "Frontegg client is not authenticated");
            }
            
            requestMessage.AddAuthorizationHeader(latestState.Token);
            
            // TODO set timeout from configuration.
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
        
        private static CancellationToken GetCancellationTokenForRequest(TimeSpan timeout, CancellationToken cancellationToken)
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