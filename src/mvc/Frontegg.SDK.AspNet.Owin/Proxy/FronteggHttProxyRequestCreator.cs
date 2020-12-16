using System;
using System.Linq;
using System.Net.Http;
using Frontegg.SDK.AspNet.Owin.Http;
using Microsoft.Owin;

namespace Frontegg.SDK.AspNet.Owin.Proxy
{
    internal class FronteggHttProxyRequestCreator : IFronteggHttProxyRequestCreator
    {
        public HttpRequestMessage CreateProxyRequest(IOwinContext context, ProxyHttpRequestData data)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (data == null) throw new ArgumentNullException(nameof(data));
            var proxyRequest = CreateProxyHttpRequest(context.Request);
            var relativeUrl = GetFronteggOriginPathAndQuery(context.Request);
            proxyRequest.RequestUri = relativeUrl;
            PopulateFronteggData(proxyRequest, data);
            proxyRequest.Headers.Host = data.FronteggUrl.Authority;
            return proxyRequest;
        }

        private static Uri GetFronteggOriginPathAndQuery(IOwinRequest request, string removedSegment = "/frontegg")
        {
            var encodedPathAndQuery = request.Path.Value;
            var originPathAndQuery = encodedPathAndQuery.Replace(removedSegment, "");
            return new Uri(originPathAndQuery, UriKind.Relative);
        }
        
        private static void PopulateFronteggData(HttpRequestMessage proxyRequest, ProxyHttpRequestData data)
        {
            if (proxyRequest == null) throw new ArgumentNullException(nameof(proxyRequest));
            proxyRequest.Headers.Add(Frontegg.AccessTokenHeaderKey, data.Token);
            proxyRequest.Headers.Add(Frontegg.TenantIdHeaderKey, data.TenantId);
            proxyRequest.Headers.Add(Frontegg.UserIdHeaderKey, data.UserId);
        }
        
        private static HttpRequestMessage CreateProxyHttpRequest(IOwinRequest request)
        {
            var requestMessage = new HttpRequestMessage();

            if (request.Body.Length != 0)
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }

            // check if we need to copy all the headers ? 
            foreach (var header in request.Headers)
            {
                if (header.Key.StartsWith("X-Forwarded-", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
                
                try
                {
                    requestMessage.Headers.TryGetValues("User-Agent", out _);
                }
                catch (IndexOutOfRangeException)
                {
                    requestMessage.Headers.Remove("User-Agent");
                }

                requestMessage.Method = new HttpMethod(request.Method);
            }

            return requestMessage;
        }
    }
}