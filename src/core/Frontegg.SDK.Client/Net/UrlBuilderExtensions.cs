using System.Collections.Generic;

namespace Frontegg.SDK.Client.Net
{
    internal static class UrlBuilderExtensions
    {
        public class UrlBuilderQueryPair
        {
            public UrlBuilderQueryPair(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; set; }

            public string Value { get; set; }
        }
        
        public static UrlBuilder ToUrlBuilder(this string url)
        {
            return new UrlBuilder(url);
        }

        public static UrlBuilder WithQueryParam(this UrlBuilder urlBuilder, string key, string value)
        {
            urlBuilder.Query.AddOrUpdate(key, value);
            return urlBuilder;
        }

        public static UrlBuilder WithQueryParam<T>(this UrlBuilder urlBuilder, string key, T value)
        {
            urlBuilder.Query.AddOrUpdate(key, value?.ToString());
            return urlBuilder;
        }


        public static UrlBuilder WithCollectionQueryParam(this UrlBuilder urlBuilder, string key, ICollection<string> value)
        {
            urlBuilder.Query.AddOrUpdateCollection(key, value);
            return urlBuilder;
        }

        public static UrlBuilder Secured(this UrlBuilder urlBuilder)
        {
            urlBuilder.Protocol = "https";
            return urlBuilder;
        }

        public static UrlBuilder WithQueryParamsFromAnotherUrl(this UrlBuilder target, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return target;
            }

            var otherUrlBuilder = new UrlBuilder(url);
            foreach (string key in otherUrlBuilder.Query.Values.Keys)
            {
                var value = otherUrlBuilder.Query.GetValue(key);
                target.Query.AddOrUpdate(key, value);
            }

            return target;
        }

        public static UrlBuilder WithProtocol(this UrlBuilder builder, string protocol)
        {
            builder.Protocol = protocol;
            return builder;
        }

        public static UrlBuilder WithHost(this UrlBuilder builder, string host)
        {
            builder.Host = host;
            return builder;
        }

        public static UrlBuilder WithPath(this UrlBuilder builder, string path)
        {
            builder.Path = path;
            return builder;
        }

        public static UrlBuilder WithPort(this UrlBuilder builder, int? port)
        {
            builder.Port = port;
            return builder;
        }

        public static UrlBuilder WithHttp(this UrlBuilder builder, bool secure)
        {
            builder.Protocol = secure ? "https" : "http";
            return builder;
        }
    }
}