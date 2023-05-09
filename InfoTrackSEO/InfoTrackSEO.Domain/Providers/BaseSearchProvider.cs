using System.Collections.Generic;
using InfoTrackSEO.Domain.Models;

public abstract class BaseSearchProvider : ISearchProvider
{
    public BaseSearchProvider(string searchProvider, string url, KeyValuePair<string, string>? apiKey = null)
    {
        _uri = new Uri(url);
        _searchProvider = searchProvider;
        _apiKey = apiKey;
    }

    private readonly Uri _uri;
    private readonly string _searchProvider;
    protected readonly KeyValuePair<string, string>? _apiKey;
    
    public async Task<SearchResult> GetSearchResultAsync(string keywords, string targetUrl)
    {
        var searchResult = new SearchResult(new CreateSearchResult(_searchProvider, DateTime.Now, keywords, targetUrl));

        var searchResultContents = await RunSearch(keywords, targetUrl) ?? string.Empty;
        var results = ParseSearchResults(searchResultContents, targetUrl);

        searchResult.SetResults(results);

        return searchResult;
    }

    protected async Task<string?> RunSearch(string keywords, string targetUrl)
    {
        using var httpClient = new HttpClient();

        var apiKeyHeaderKey = _apiKey.HasValue ? _apiKey.Value.Key : null;
        var apiKeyHeaderValue = _apiKey.HasValue ? _apiKey.Value.Value : null;

        if(!string.IsNullOrWhiteSpace(apiKeyHeaderKey) && !string.IsNullOrWhiteSpace(apiKeyHeaderValue))
        {
            httpClient.DefaultRequestHeaders.Add(apiKeyHeaderKey, apiKeyHeaderValue);
        }

        var url = _uri.AbsoluteUri + keywords;
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return content;
    }

    protected abstract IEnumerable<Result> ParseSearchResults(string searchContent, string targetUrl);
}