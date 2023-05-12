using System.Text.RegularExpressions;
using InfoTrackSEO.Domain.Repositories;

public class GoogleSearchProvider : BaseSearchProvider
{
    public GoogleSearchProvider(
        ISearchResultRepository searchResultRepository,
        IHttpClientFactory httpClientFactory
    )
        : base(
            "Google",
            "https://www.google.com/search?num=100&q=",
            searchResultRepository,
            httpClientFactory
        ) { }

    protected override IEnumerable<Uri> GetSearchResultLinks(string document)
    {
        var hyperlinkRegex = new Regex(@"<a href=""/url\?q=(.*?)"">", RegexOptions.IgnoreCase);
        var searchResults = hyperlinkRegex.Matches(document);

        foreach (Match result in searchResults)
        {
            var resultUrl = result.Groups.Values.LastOrDefault()?.Value ?? string.Empty;
            yield return new Uri(resultUrl);
        }
    }
}
