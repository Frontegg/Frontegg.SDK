using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Frontegg.SDK.Client.Extensions;

namespace Frontegg.SDK.Client.Net
{
    internal class UrlBuilderQuery
    {
        private static readonly string ArrayKeySuffix = Uri.EscapeDataString("[]");

        public UrlBuilderQuery()
        {
            Values = new OrderedDictionary();
        }

        public UrlBuilderQuery(string query)
            : this()
        {
            if (query.StartsWith("?"))
            {
                query = query.Substring(1);
            }

            var pairs = query.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);

            pairs.Where(x => !x.StartsWith("="))
                .Select(x => x.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(x => x[0].EndsWith(ArrayKeySuffix) ? x[0].ChopTail(ArrayKeySuffix.Length) : x[0])
                .ForEach(group =>
                {
                    if (group.Count() == 1)
                    {
                        var pair = group.First();
                        var value = pair.Length == 2 ? Uri.UnescapeDataString(pair[1]) : null;
                        AddOrUpdate(group.Key, value);
                        return;
                    }

                    AddOrUpdateCollection(group.Key, group.Select(pair => pair.Length == 2 ? Uri.UnescapeDataString(pair[1]) : null).ToList());
                });
        }

        public OrderedDictionary Values { get; }

        public bool AddOrUpdateCollection(string key, ICollection<string> value)
        {
            return AddOrUpdateInternal(key, value);
        }

        public bool AddOrUpdate(string key, string value)
        {
            return AddOrUpdateInternal(key, value);
        }

        private bool AddOrUpdateInternal(string key, object value)
        {
            if (Values.Contains(key))
            {
                Values[key] = value;
                return true;
            }

            Values.Add(key, value);
            return false;
        }

        public string GetValue(string key)
        {
            var value = Values[key];
            return value?.ToString();
        }

        public ICollection<string> GetValues(string key)
        {
            var value = Values[key];
            return (ICollection<string>) value;
        }

        public override string ToString()
        {
            if (Values.Count == 0)
                return string.Empty;

            var formattedPairs = Values.Keys
                .Cast<string>()
                .SelectMany(x => GetFormattedPairs(x, Values[x]))
                .ToArray();

            return "?" + string.Join("&", formattedPairs);
        }

        private static IEnumerable<string> GetFormattedPairs(string key, object value)
        {
            if (value is ICollection<string> collection)
            {
                return collection.Select(x => FormatPair(key + "[]", x));
            }
            return FormatPair(key, value).AsArray();
        }

        private static string FormatPair(string key, object value)
        {
            key = Uri.EscapeDataString(key);
            if (value == null)
                return key;

            return $"{key}={Uri.EscapeDataString(Convert.ToString(value))}";
        }
    }
}