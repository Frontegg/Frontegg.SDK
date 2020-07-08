using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Frontegg.SDK.AspNet.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RichardSzalay.MockHttp;
using Xunit;

namespace Frontegg.SDK.AspNet.Tests.UnitTests
{
    public class FronteggHttpRequestProxySenderTests
    {
        [Theory]
        [MemberData(nameof(FronteggHttpRequestProxyData))]
        public async void Test(Exception exception, HttpStatusCode expected)
        {
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var logger = Substitute.For<ILogger<FronteggHttpRequestProxySender>>();
            var sut = new FronteggHttpRequestProxySender(httpClientFactory, logger);

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*")
                .Throw(exception);

            httpClientFactory.CreateClient(Arg.Any<string>())
                .Returns(mockHttp.ToHttpClient());

            var message = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://test.com")
            };

            var result = await sut.ProxyHttpRequest(message, new DefaultHttpContext())
                .ConfigureAwait(false);

            Assert.Equal(expected, result.StatusCode);
        }

        public static IEnumerable<object[]> FronteggHttpRequestProxyData =>
            new List<object[]>
            {
                new object[]
                    {new TaskCanceledException("SomeMessage", new IOException()), HttpStatusCode.GatewayTimeout},
                new object[]
                    {new OperationCanceledException("SomeMessage"), HttpStatusCode.ServiceUnavailable},
                new object[]
                    {new HttpRequestException("SomeMessage", new IOException()), HttpStatusCode.ServiceUnavailable},
                new object[]
                    {new HttpRequestException("SomeMessage", new SocketException()), HttpStatusCode.ServiceUnavailable}
            };
    }
}