using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Frontegg.SDK.AspNet.Owin.Extensions
{
    public static class HttpContentExtensions
    {
        public static HttpContent AddJsonContentType(this HttpContent target, Action<MediaTypeHeaderValue> action = null)
        {
            var mediaType = new MediaTypeHeaderValue("application/json");
            action?.Invoke(mediaType);
            target.Headers.ContentType = mediaType;
            return target;
        }

        public static HttpContent AddJsonUtf8ContentType(this HttpContent target)
        {
            target.AddJsonContentType(x => x.CharSet = Encoding.UTF8.WebName);
            return target;
        }
    }
}