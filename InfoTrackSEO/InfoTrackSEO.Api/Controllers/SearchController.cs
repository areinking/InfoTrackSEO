using InfoTrackSEO.Domain.Repositories;
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
        private readonly ISearchResultRepository _searchResultRepository;

        public SearchController(
            ILogger<SearchController> logger,
            SearchProviderFactory searchProviderFactory,
            ISearchResultRepository searchResultRepository
        )
        {
            _logger = logger;
            _searchProviderFactory = searchProviderFactory;
            _searchResultRepository = searchResultRepository;
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

        [HttpGet("GetRange")]
        public async Task<IActionResult> GetRange(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Starting search result range operation.");

            try
            {
                var results = await _searchResultRepository.GetByDateRangeAsync(startDate, endDate);
                var searchResultDtos = results
                    .Select(
                        result =>
                            new SearchResultDto
                            {
                                Positions = result.ToString(),
                                Keywords = result.Keywords,
                                SearchEngine = result.SearchProvider,
                                SearchDate = result.SearchDate.ToShortDateString(),
                                URL = result.TargetUrl,
                                Results = result.Results
                            }
                    )
                    .ToList();

                return Ok(searchResultDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the search result range operation.");
                return StatusCode(
                    500,
                    "An error occurred during the search result range operation. Please try again."
                );
            }
        }
    }
}
