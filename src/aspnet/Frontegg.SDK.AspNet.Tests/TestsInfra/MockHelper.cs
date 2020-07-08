using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;

namespace Frontegg.SDK.AspNet.Tests.UnitTests
{
    public static class MockHelper
    {
        public static void Mock<T>(out T obj) where T : class
        {
            obj = Substitute.For<T>();
        }
        
        public static IFixture GetFixture()
        {
            return new Fixture().Customize(new AutoNSubstituteCustomization()); 
        }
    }
}