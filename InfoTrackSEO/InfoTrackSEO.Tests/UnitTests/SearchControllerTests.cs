using System.Threading.Tasks;
using InfoTrackSEO.API.Controllers;
using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Models;
using InfoTrackSEO.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InfoTrackSEO.Tests.UnitTests
{
    public class SearchControllerTests
    {
        private readonly SearchController _controller;
        private readonly Mock<ILogger<SearchController>> _loggerMock;
        private readonly SearchProviderFactory _searchProviderFactory;

        public SearchControllerTests()
        {
            _loggerMock = new Mock<ILogger<SearchController>>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(c => c.CreateClient(string.Empty))
                .Returns(new Mock<HttpClient>().Object);
            var googleSearchProvider = new GoogleSearchProvider(
                new Mock<ISearchResultRepository>().Object,
                httpClientFactoryMock.Object,
                new EventBus() // simple console write line, so it's okay not being mocked
            );
            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock
                .Setup(p => p.GetService(typeof(GoogleSearchProvider)))
                .Returns(googleSearchProvider);

            _searchProviderFactory = new SearchProviderFactory(serviceProviderMock.Object);
            _controller = new SearchController(_loggerMock.Object, _searchProviderFactory);
        }

        [Theory]
        [InlineData("Google", "test", "test", typeof(OkObjectResult))]
        [InlineData("Foo", "test", "test", typeof(BadRequestObjectResult))]
        [InlineData("", "test", "test", typeof(BadRequestObjectResult))]
        [InlineData("test", "", "test", typeof(BadRequestObjectResult))]
        [InlineData("test", "test", "", typeof(BadRequestObjectResult))]
        public async void ControllerResultTheory(
            string searchEngine,
            string keywords,
            string url,
            Type expectedType
        )
        {
            // Arrange
            var searchRequest = new SearchRequest
            {
                SearchEngine = searchEngine,
                Keywords = keywords,
                Url = url
            };

            // Act
            var result = await _controller.Post(searchRequest);

            // Assert
            Assert.IsType(expectedType, result);
        }
    }
}
