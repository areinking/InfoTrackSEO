using System;
using System.Threading.Tasks;
using InfoTrackSEO.Domain.DomainServices;
using InfoTrackSEO.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoTrackSEO.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly SearchServiceFactory _searchServiceFactory;

        public SearchController(ILogger<SearchController> logger, SearchServiceFactory searchServiceFactory)
        {
            _logger = logger;
            _searchServiceFactory = searchServiceFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string keywords, string url, string searchEngine)
        {
            _logger.LogInformation("Starting search operation.");

            try
            {
                var searchService = _searchServiceFactory.CreateSearchService(searchEngine);
                var result = await searchService.SearchAsync(keywords, url, searchEngine);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during search operation.");
                return StatusCode(500, "An error occurred during the search operation. Please try again.");
            }
        }
    }
}
