using InfoTrackSEO.Domain.DomainServices;
using Moq;
using Xunit;

namespace InfoTrackSEO.Tests.UnitTests
{
    public class SearchServiceFactoryTests
    {
        private readonly SearchServiceFactory _factory;

        public SearchServiceFactoryTests()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(GoogleSearchService)))
                .Returns(new GoogleSearchService());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(BingSearchService)))
                .Returns(new BingSearchService());
            _factory = new SearchServiceFactory(serviceProviderMock.Object);
        }

        [Fact]
        public void CreateSearchService_GivenGoogle_ReturnsGoogleSearchService()
        {
            // Act
            var service = _factory.CreateSearchService("Google");

            // Assert
            Assert.IsType<GoogleSearchService>(service);
        }

        [Fact]
        public void CreateSearchService_GivenBing_ReturnsBingSearchService()
        {
            // Act
            var service = _factory.CreateSearchService("Bing");

            // Assert
            Assert.IsType<BingSearchService>(service);
        }

        [Fact]
        public void CreateSearchService_GivenInvalidEngine_ThrowsArgumentException()
        {
            // Act and Assert
            Assert.Throws<System.ArgumentException>(() => _factory.CreateSearchService("InvalidEngine"));
        }
    }
}
