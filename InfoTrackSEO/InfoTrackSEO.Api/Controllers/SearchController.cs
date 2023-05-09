using System;
using System.Threading.Tasks;
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
        private readonly SearchProviderFactory _searchProviderFactory;

        public SearchController(ILogger<SearchController> logger, SearchProviderFactory searchProviderFactory)
        {
            _logger = logger;
            _searchProviderFactory = searchProviderFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string keywords, string url, string searchEngine)
        {
            _logger.LogInformation("Starting search operation.");

            try {
                var searchService = _searchProviderFactory.GetSearchProvider(searchEngine);
                var result = await searchService.GetSearchResultAsync(keywords, url);
                return Ok(result.ToString());
            }
            catch(ArgumentException argEx) {
                _logger.LogError(argEx, "An error occurred during search operation.");
                return BadRequest(argEx);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred during search operation.");
                return StatusCode(500, "An error occurred during the search operation. Please try again.");
            }
        }
    }
}
