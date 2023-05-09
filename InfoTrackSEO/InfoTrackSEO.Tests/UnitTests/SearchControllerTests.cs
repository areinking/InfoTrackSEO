using System.Threading.Tasks;
using InfoTrackSEO.API.Controllers;
using InfoTrackSEO.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InfoTrackSEO.Tests.UnitTests
{
    public class SearchControllerTests
    {
        private readonly SearchController _controller;
        private readonly Mock<ILogger<SearchController>> _loggerMock;
        private readonly Mock<SearchProviderFactory> _searchProviderFactoryMock;

        public SearchControllerTests()
        {
            _loggerMock = new Mock<ILogger<SearchController>>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(GoogleSearchProvider)))
                .Returns(new GoogleSearchProvider());
            _searchProviderFactoryMock = new Mock<SearchProviderFactory>(serviceProviderMock.Object);
            _controller = new SearchController(_loggerMock.Object, _searchProviderFactoryMock.Object);
        }

        [Fact]
        public async Task Get_WhenCalled_Google_ReturnsOkResult()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";
            string searchEngine = "Google";

            var searchServiceMock = new Mock<ISearchProvider>();
            var createSearchResult = new Mock<CreateSearchResult>("",DateTime.Now,"","").Object;
            var searchResult = new Mock<SearchResult>(createSearchResult).Object;
            searchServiceMock.Setup(service => service.GetSearchResultAsync(keywords, url))
                .ReturnsAsync(searchResult);

            // Act
            var result = await _controller.Post(keywords, url, searchEngine);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task Get_WhenCalled_Foo_ReturnsBadRequestResult()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";
            string searchEngine = "foo";

            var searchServiceMock = new Mock<ISearchProvider>();
            var createSearchResult = new Mock<CreateSearchResult>("",DateTime.Now,"","").Object;
            var searchResult = new Mock<SearchResult>(createSearchResult).Object;
            searchServiceMock.Setup(service => service.GetSearchResultAsync(keywords, url))
                .ReturnsAsync(searchResult);

            // Act
            var result = await _controller.Post(keywords, url, searchEngine);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
