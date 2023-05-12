using System.Text.RegularExpressions;
using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Repositories;

public class GoogleSearchProvider : BaseSearchProvider
{
    public GoogleSearchProvider(
        ISearchResultRepository searchResultRepository,
        IHttpClientFactory httpClientFactory,
        IEventBus eventBus
    )
        : base(
            "Google",
            "https://www.google.com/search?num=100&q=",
            searchResultRepository,
            httpClientFactory,
            eventBus
        ) { }

    protected override IEnumerable<Uri> GetSearchResultLinks(string document)
    {
        // their links look like <a href="/url?q=Some.Url.Here">
        var hyperlinkRegex = new Regex(@"<a href=""/url\?q=(.*?)"">", RegexOptions.IgnoreCase);
        var searchResults = hyperlinkRegex.Matches(document);

        foreach (Match result in searchResults)
        {
            // the parenthesis in the regex group the values, there is only one we
            // care about at the end.  The other is the whole match.
            var resultUrl = result.Groups.Values.LastOrDefault()?.Value ?? string.Empty;
            yield return new Uri(resultUrl);
        }
    }
}
