using System;
using System.Collections.Generic;
using System.Net.Http;
using Frontegg.SDK.AspNet.Proxy;
using Frontegg.SDK.AspNet.Tests.TestsInfra;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Xunit;

namespace Frontegg.SDK.AspNet.Tests.UnitTests
{
    public class FronteggHttProxyRequestCreatorTests
    {
        [Fact]
        public void CreateProxyRequest_WhenCreatingProxyRequest_ShouldCopyAllData()
        {
            var header1 = new KeyValuePair<string, StringValues>("x-custom-header-1", "1");
            var header2 = new KeyValuePair<string, StringValues>("x-custom-header-2", "2");
            var tokenHeader = new KeyValuePair<string, StringValues>(Frontegg.AccessTokenHeaderKey, Guid.NewGuid().ToString());
            var tenantIdHeader = new KeyValuePair<string, StringValues>(Frontegg.TenantIdHeaderKey, Guid.NewGuid().ToString());
            var userIdHeader = new KeyValuePair<string, StringValues>(Frontegg.UserIdHeaderKey, Guid.NewGuid().ToString());
            
            const string path = "/frontegg/path";
            const string expectedPath = "/path";
            
            var sut = new FronteggHttProxyRequestCreator();
            var context = new DefaultHttpContext();
            var request = context.Request;

            request.Method = HttpMethod.Get.Method;
            request.Headers.Add(header1);
            request.Headers.Add(header2);
            context.Request.Path = new PathString(path);
            
            var data = new ProxyHttpRequestData()
            {
                Token = tokenHeader.Value,
                FronteggUrl = new Uri("http://www.frontegg.com/frontegg/one"),
                TenantId = tenantIdHeader.Value,
                UserId = userIdHeader.Value
            };
            
            var result = sut.CreateProxyRequest(context, data);
                
            result.Headers.AssertHeader(header1.ToIEnumerableValues());
            result.Headers.AssertHeader(header2.ToIEnumerableValues());
            result.Headers.AssertHeader(tokenHeader.ToIEnumerableValues());
            result.Headers.AssertHeader(tenantIdHeader.ToIEnumerableValues());
            result.Headers.AssertHeader(userIdHeader.ToIEnumerableValues());

            result.Method.ShouldBe(HttpMethod.Get);
            result.RequestUri.ShouldBe(new Uri(expectedPath, UriKind.Relative));
            result.Headers.Host.ShouldBe(data.FronteggUrl.Authority);
        }
    }
}