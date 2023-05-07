using System.Threading.Tasks;
using InfoTrackSEO.Domain.DomainServices;
using Xunit;

namespace InfoTrackSEO.Tests.IntegrationTests
{
    public class SearchServiceTests
    {
        private readonly GoogleSearchService _googleSearchService;
        private readonly BingSearchService _bingSearchService;

        public SearchServiceTests()
        {
            _googleSearchService = new GoogleSearchService();
            _bingSearchService = new BingSearchService();
        }

        [Fact]
        public async Task GoogleSearchService_WhenCalled_ReturnsPositions()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";

            // Act
            var result = await _googleSearchService.SearchAsync(keywords, url, "Google");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task BingSearchService_WhenCalled_ReturnsPositions()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";

            // Act
            var result = await _bingSearchService.SearchAsync(keywords, url, "Bing");

            // Assert
            Assert.NotNull(result);
        }
    }
}
