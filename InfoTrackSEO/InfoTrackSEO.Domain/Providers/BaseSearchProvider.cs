using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Models;
using InfoTrackSEO.Domain.Repositories;

public abstract class BaseSearchProvider : ISearchProvider
{
    public BaseSearchProvider(
        string searchProvider,
        string url,
        ISearchResultRepository searchResultRepository,
        IHttpClientFactory httpClientFactory,
        IEventBus eventBus,
        KeyValuePair<string, string>? apiKey = null
    )
    {
        _uri = new Uri(url);
        _searchProvider = searchProvider;
        _searchResultRepository = searchResultRepository;
        _httpClientFactory = httpClientFactory;
        _eventBus = eventBus;
        _apiKey = apiKey;
    }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEventBus _eventBus;
    private readonly Uri _uri;
    private readonly string _searchProvider;
    protected readonly KeyValuePair<string, string>? _apiKey;
    private ISearchResultRepository _searchResultRepository;

    public async Task<SearchResult> RunSearchRequestAsync(string keywords, string targetUrl)
    {
        // create the domain object
        var searchResult = new SearchResult().Create(
            new CreateSearchResult(_searchProvider, DateTime.Now, keywords, targetUrl)
        );

        // save the domain object and raise an event for other systems to know about it
        await _searchResultRepository.AddAsync(searchResult);
        var createSearchResultDomainEvent = new CreateSearchResultDomainEvent
        {
            SearchResult = searchResult
        };
        await _eventBus.AsyncPublish(createSearchResultDomainEvent);

        // run the search and update the domain model to have the raw results
        var searchResultContents = await RunSearch(keywords, targetUrl) ?? string.Empty;
        searchResult.SetDocument(searchResultContents);

        // parse out the links and add meta data that we need for the user
        var results = ParseSearchResults(searchResultContents, targetUrl).Take(100).ToList();
        searchResult.SetResults(results);

        // update the domain model in the db and raise an event for all to know it changed
        await _searchResultRepository.UpdateAsync(searchResult);
        var updateSearchResultDomainEvent = new UpdateSearchResultDomainEvent
        {
            SearchResult = searchResult
        };
        await _eventBus.AsyncPublish(updateSearchResultDomainEvent);

        return searchResult;
    }

    protected async Task<string?> RunSearch(string keywords, string targetUrl)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        // other providers need an api key (like bing)
        // I would have added bing as a provider, but there was too much setup
        var apiKeyHeaderKey = _apiKey.HasValue ? _apiKey.Value.Key : null;
        var apiKeyHeaderValue = _apiKey.HasValue ? _apiKey.Value.Value : null;

        if (
            !string.IsNullOrWhiteSpace(apiKeyHeaderKey)
            && !string.IsNullOrWhiteSpace(apiKeyHeaderValue)
        )
        {
            // if there is an api key needed, add it to the headers
            httpClient.DefaultRequestHeaders.Add(apiKeyHeaderKey, apiKeyHeaderValue);
        }

        var url = _uri.AbsoluteUri + keywords;
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return content;
    }

    protected IEnumerable<LinkPosition> ParseSearchResults(string searchContent, string targetUrl)
    {
        var positions = new List<int>();
        int position = 1;

        foreach (var resultUrl in GetSearchResultLinks(searchContent))
        {
            yield return new LinkPosition
            {
                Url = resultUrl.Host,
                Position = position,
                IsHit = resultUrl.Host.Contains(targetUrl)
            };

            position++;
        }
    }

    protected abstract IEnumerable<Uri> GetSearchResultLinks(string document);
}
