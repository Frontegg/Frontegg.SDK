using System.Collections.Generic;
using System.Net.Http.Headers;
using Shouldly;

namespace Frontegg.SDK.AspNet.Tests.TestsInfra
{
    public static class HttpHeadersTestsExtensions
    {
        public static void AssertHeader(this HttpRequestHeaders target, KeyValuePair<string, IEnumerable<string>> header)
        {
            target.Contains(header.Key).ShouldBeTrue();
            target.GetValues(header.Key).ShouldBe(header.Value, Case.Sensitive);
        }
    }
}