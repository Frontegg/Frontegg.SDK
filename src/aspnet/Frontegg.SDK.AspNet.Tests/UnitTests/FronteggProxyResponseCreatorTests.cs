using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Frontegg.SDK.AspNet.Authentication;
using Frontegg.SDK.AspNet.Proxy;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;
using static Frontegg.SDK.AspNet.Tests.TestsInfra.MockHelper;

namespace Frontegg.SDK.AspNet.Tests.UnitTests
{
    public class FronteggProxyResponseCreatorTests
    {
        private const string Token = "token";
        private const int ExpiredId = 1800;
        private const string TenantId = "TenantId";
        private const string UserId = "UserId";
        
        [Fact]
        public async Task GenerateProxyResponse_WhenAuthenticationStateIsNotAuthorize_ShouldReturnForbiddenCode()
        {
            var fixture = GetFixture();
            var authenticationStateStore = fixture.Freeze<IAuthenticationStateStore>();
            authenticationStateStore.GetLatestState().Returns(FronteggAuthenticationResult.FailedResult("SomeReason"));
            
            var sut = fixture.Create<FronteggProxyResponseCreator>();
            var context = new DefaultHttpContext();
            
            var result = await sut.GenerateProxyResponse(context);
           
            result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        private IFixture GetFixtureWithAuthorizedState(string token = Token, int expiredIn = ExpiredId)
        {
            var fixture = GetFixture();
            var authenticationStateStore = fixture.Freeze<IAuthenticationStateStore>();
            authenticationStateStore.GetLatestState().Returns(new FronteggAuthenticationResult(token, expiredIn));
            return fixture;
        }
    }
}