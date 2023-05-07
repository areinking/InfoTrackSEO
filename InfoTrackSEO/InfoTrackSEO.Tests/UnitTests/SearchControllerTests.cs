using System.Threading.Tasks;
using InfoTrackSEO.API.Controllers;
using InfoTrackSEO.Domain.DomainServices;
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
        private readonly Mock<SearchServiceFactory> _searchServiceFactoryMock;

        public SearchControllerTests()
        {
            _loggerMock = new Mock<ILogger<SearchController>>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(GoogleSearchService)))
                .Returns(new GoogleSearchService());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(BingSearchService)))
                .Returns(new BingSearchService());
            _searchServiceFactoryMock = new Mock<SearchServiceFactory>(serviceProviderMock.Object);
            _controller = new SearchController(_loggerMock.Object, _searchServiceFactoryMock.Object);
        }

        [Fact]
        public async Task Get_WhenCalled_Google_ReturnsOkResult()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";
            string searchEngine = "Google";

            var searchServiceMock = new Mock<ISearchService>();
            searchServiceMock.Setup(service => service.SearchAsync(keywords, url, searchEngine))
                .ReturnsAsync(new SearchResult{ Positions= "1, 10, 33" });

            // Act
            var result = await _controller.Get(keywords, url, searchEngine);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_WhenCalled_Bing_ReturnsOkResult()
        {
            // Arrange
            string keywords = "efiling integration";
            string url = "www.infotrack.com";
            string searchEngine = "Bing";

            var searchServiceMock = new Mock<ISearchService>();
            searchServiceMock.Setup(service => service.SearchAsync(keywords, url, searchEngine))
                .ReturnsAsync(new SearchResult{ Positions= "1, 10, 33" });

            // Act
            var result = await _controller.Get(keywords, url, searchEngine);

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

            var searchServiceMock = new Mock<ISearchService>();
            searchServiceMock.Setup(service => service.SearchAsync(keywords, url, searchEngine))
                .ReturnsAsync(new SearchResult{ Positions= "1, 10, 33" });

            // Act
            var result = await _controller.Get(keywords, url, searchEngine);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
