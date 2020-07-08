using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Frontegg.SDK.AspNet.Http
{
    internal static class HttpContextExtensions
    {
        public static Uri GetFronteggOriginPathAndQuery(this HttpRequest request, string removedSegment = "/frontegg")
        {
            var encodedPathAndQuery = request.GetEncodedPathAndQuery();
            var originPathAndQuery = encodedPathAndQuery.Replace(removedSegment, "");
            return new Uri(originPathAndQuery, UriKind.Relative);
        }
        
        public static HttpRequestMessage CreateProxyHttpRequest(this HttpRequest request)
        {
            var requestMessage = new HttpRequestMessage();
            
            if (request.ContentLength.HasValue)
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