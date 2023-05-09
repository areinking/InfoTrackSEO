using System.Text.RegularExpressions;

public class GoogleSearchProvider : BaseSearchProvider
{
    public GoogleSearchProvider() : base("Google", "https://www.google.com/search?num=100&q=")
    {
    }

    protected override IEnumerable<Result> ParseSearchResults(string searchContent, string targetUrl)
    {
        var searchResults = GetSearchResultLinks(searchContent);
        var positions = new List<int>();
        int position = 1;

        foreach (Match result in searchResults)
        {
            var resultUrl = result.Groups.Values.LastOrDefault()?.Value ?? string.Empty;

            if (resultUrl.Contains(targetUrl))
            {
                yield return new Result { };
            }

            position++;
        }

        if (!positions.Any()) {
            yield return new Result {  };
        }
    }

    protected MatchCollection GetSearchResultLinks(string document)
    {
        var hyperlinkRegex = new Regex(@"<a href=""/url\?q=(.*?)"">", RegexOptions.IgnoreCase);
        var searchResults = hyperlinkRegex.Matches(document);
        return searchResults;
    }
}