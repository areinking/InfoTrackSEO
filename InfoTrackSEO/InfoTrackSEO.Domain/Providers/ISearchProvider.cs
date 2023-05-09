using InfoTrackSEO.Domain.Models;

public interface ISearchProvider
{
    Task<SearchResult> GetSearchResultAsync(string keywords, string targetUrl);
}