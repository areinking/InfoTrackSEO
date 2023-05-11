using InfoTrackSEO.Domain.Models;

public interface ISearchProvider
{
    Task<SearchResult> RunSearchRequestAsync(string keywords, string targetUrl);
}
