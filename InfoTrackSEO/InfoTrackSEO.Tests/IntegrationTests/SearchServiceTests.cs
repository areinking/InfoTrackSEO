using System.Threading.Tasks;
using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Repositories;
using Moq;
using Xunit;

namespace InfoTrackSEO.Tests.IntegrationTests
{
    public class SearchProviderTests
    {
        private readonly GoogleSearchProvider _googleSearchProvider;

        public SearchProviderTests()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(c => c.CreateClient(string.Empty))
                .Returns(new Mock<HttpClient>().Object);
            _googleSearchProvider = new GoogleSearchProvider(
                new Mock<ISearchResultRepository>().Object,
                httpClientFactoryMock.Object,
                new EventBus() // simple console write line, so it's okay not being mocked
            );
        }

        [Fact]
        public async Task GoogleSearchService_WhenCalled_ReturnsPositions()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";

            // Act
            var result = await _googleSearchProvider.RunSearchRequestAsync(keywords, url);

            // Assert
            Assert.NotNull(result);
        }
    }
}
