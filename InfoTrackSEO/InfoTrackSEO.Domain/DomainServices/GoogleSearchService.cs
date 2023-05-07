using System;
using System.Threading.Tasks;
using InfoTrackSEO.Domain.Models;

namespace InfoTrackSEO.Domain.DomainServices
{
    public class GoogleSearchService : BaseSearchService, ISearchService
    {
        public async Task<SearchResult> SearchAsync(string keywords, string url, string searchEngine)
        {
            var encodedKeywords = Uri.EscapeDataString(keywords);
            var searchUrl = $"https://www.google.com/search?num=100&q={encodedKeywords}";
            var document = await FetchSearchResultsAsync(searchUrl);
            var positions = GetPositions(document, url);

            return new SearchResult
            {
                Keywords = keywords,
                Url = url,
                SearchEngine = searchEngine,
                Positions = string.Join(", ", positions),
                SearchDate = DateTime.UtcNow
            };
        }
    }
}
