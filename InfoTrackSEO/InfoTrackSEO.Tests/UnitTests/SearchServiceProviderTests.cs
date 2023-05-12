using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Repositories;
using Moq;

namespace InfoTrackSEO.Tests.UnitTests
{
    public class SearchProviderFactoryTests
    {
        private readonly SearchProviderFactory _factory;

        public SearchProviderFactoryTests()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(GoogleSearchProvider)))
                .Returns(
                    new GoogleSearchProvider(
                        new Mock<ISearchResultRepository>().Object,
                        new Mock<IHttpClientFactory>().Object,
                        new EventBus() // simple console write line, so it's okay not being mocked
                    )
                );
            _factory = new SearchProviderFactory(serviceProviderMock.Object);
        }

        [Fact]
        public void CreateSearchService_GivenGoogle_ReturnsGoogleSearchService()
        {
            // Act
            var service = _factory.GetSearchProvider("Google");

            // Assert
            Assert.IsType<GoogleSearchProvider>(service);
        }

        [Fact]
        public void CreateSearchService_GivenInvalidEngine_ThrowsArgumentException()
        {
            // Act and Assert
            Assert.Throws<System.ArgumentException>(
                () => _factory.GetSearchProvider("InvalidEngine")
            );
        }
    }
}
