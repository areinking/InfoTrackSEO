using System;
using System.Threading.Tasks;
using InfoTrackSEO.Domain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoTrackSEO.API.Controllers
{
    public class SearchResultDto {
        public string? Keywords { get; set; }
        public string? URL { get; set; }
        public string? Positions { get; set; }
        public string? SearchDate { get; set; }
        public string? SearchEngine { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    [EnableCors("cors")]
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
        public async Task<IActionResult> Post([FromBody] SearchRequest searchRequest)
        {
            _logger.LogInformation("Starting search operation.");
            string keywords = searchRequest.Keywords ?? string.Empty;
            string url = searchRequest.Url ?? string.Empty;
            string searchEngine = searchRequest.SearchEngine ?? string.Empty;

            try {
                var searchService = _searchProviderFactory.GetSearchProvider(searchEngine);
                var result = await searchService.GetSearchResultAsync(keywords, url);
                return Ok(new SearchResultDto {
                    Positions = result.ToString(),
                    Keywords = Uri.UnescapeDataString(result.Keywords),
                    SearchEngine = result.SearchProvider,
                    SearchDate = result.SearchDate.ToShortDateString(),
                    URL = result.TargetUrl
                });
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
