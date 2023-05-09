using System.Threading.Tasks;
using Xunit;

namespace InfoTrackSEO.Tests.IntegrationTests
{
    public class SearchProviderTests
    {
        private readonly GoogleSearchProvider _googleSearchProvider;

        public SearchProviderTests()
        {
            _googleSearchProvider = new GoogleSearchProvider();
        }

        [Fact]
        public async Task GoogleSearchService_WhenCalled_ReturnsPositions()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";

            // Act
            var result = await _googleSearchProvider.GetSearchResultAsync(keywords, url);

            // Assert
            Assert.NotNull(result);
        }
    }
}
