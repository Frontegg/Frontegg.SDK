using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Frontegg.SDK.AspNet.Owin.Extensions
{
    internal class UrlBuilder
    {
        private const string Pattern =
            @"^(?!$)(((?<protocol>[A-z0-9]+?):(?=//))?(//)?(?<host>[^:/?]+)(:(?<port>\d+))?)?(?<path>/.*?(?=$|\?))?(?<query>(?!^)\?.+)?$";

        private static readonly Regex UrlRegex = new Regex(
            Pattern,
            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        public UrlBuilder()
        {
            Query = new UrlBuilderQuery();
        }

        public UrlBuilder(string url)
            : this()
        {
            FillDataFromUrl(url, this);
        }

        public int? Port { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public UrlBuilderQuery Query { get; set; }

        private void FillDataFromUrl(string url, UrlBuilder builder)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            var match = UrlRegex.Match(url);
            if (!match.Success)
            {
                throw new InvalidOperationException($"UrlBuilder can't parse URL [{url}]");
            }

            var groups = match.Groups;

            builder.Protocol = GetValue(groups, "protocol");
            builder.Host = GetValue(groups, "host");
            var port = GetValue(groups, "port");
            if (port != null)
            {
                builder.Port = int.Parse(port);
            }

            builder.Path = GetValue(groups, "path") ?? string.Empty;

            var query = GetValue(groups, "query");
            if (query != null)
            {
                builder.Query = new UrlBuilderQuery(query);
            }
        }

        private static string GetValue(GroupCollection groupCollection, string key)
        {
            var group = groupCollection[key];
            if (group == null)
            {
                return null;
            }

            var value = group.Value;
            return value == string.Empty ? null : value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Protocol != null)
            {
                sb.Append(Protocol + ":");
            }

            if (Host != null)
            {
                sb.Append("//" + Host);
            }

            if (Port.HasValue)
            {
                sb.Append(":" + Port);
            }

            sb.Append(GetPathAndQuery());
            return sb.ToString();
        }

        public string GetPathAndQuery()
        {
            var sb = new StringBuilder();
            if (Path != null)
            {
                sb.Append(Path.StartsWith("/") ? Path : "/" + Path);
            }

            if (Query != null)
            {
                sb.Append(Query);
            }

            return sb.ToString();
        }
    }
}