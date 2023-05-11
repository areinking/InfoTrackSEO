using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackSEO.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("cors")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly SearchProviderFactory _searchProviderFactory;

        public SearchController(
            ILogger<SearchController> logger,
            SearchProviderFactory searchProviderFactory
        )
        {
            _logger = logger;
            _searchProviderFactory = searchProviderFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchRequest searchRequest)
        {
            _logger.LogInformation("Starting search operation.");

            if (!searchRequest.IsValid())
            {
                var errorMessage =
                    "Search Request keywords, searchEngine, and Url must not be empty";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            string keywords = searchRequest.Keywords ?? string.Empty;
            string url = searchRequest.Url ?? string.Empty;
            string searchEngine = searchRequest.SearchEngine ?? string.Empty;

            try
            {
                var searchService = _searchProviderFactory.GetSearchProvider(searchEngine);
                var result = await searchService.RunSearchRequestAsync(keywords, url);
                return Ok(
                    new SearchResultDto
                    {
                        Positions = result.ToString(),
                        Keywords = keywords,
                        SearchEngine = searchEngine,
                        SearchDate = result.SearchDate.ToShortDateString(),
                        URL = url,
                        Results = result.Results
                    }
                );
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError(argEx, "An error occurred during search operation.");
                return BadRequest(argEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during search operation.");
                return StatusCode(
                    500,
                    "An error occurred during the search operation. Please try again."
                );
            }
        }
    }
}
