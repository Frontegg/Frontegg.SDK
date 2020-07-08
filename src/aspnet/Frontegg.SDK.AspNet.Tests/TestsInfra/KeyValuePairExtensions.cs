using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Frontegg.SDK.AspNet.Tests.TestsInfra
{
    public static class KeyValuePairExtensions
    {
        public static KeyValuePair<string, IEnumerable<string>> ToIEnumerableValues(this KeyValuePair<string, StringValues> target)
        {
            return new KeyValuePair<string, IEnumerable<string>>(target.Key, target.Value);
        }
    }
}