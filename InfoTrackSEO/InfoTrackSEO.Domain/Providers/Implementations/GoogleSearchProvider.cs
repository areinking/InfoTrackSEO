using System.Text.RegularExpressions;

public class GoogleSearchProvider : BaseSearchProvider
{
    public GoogleSearchProvider() : base("Google", "https://www.google.com/search?num=100&q=")
    {
    }

    protected override IEnumerable<string> GetSearchResultLinks(string document)
    {
        var hyperlinkRegex = new Regex(@"<a href=""/url\?q=(.*?)"">", RegexOptions.IgnoreCase);
        var searchResults = hyperlinkRegex.Matches(document);

        foreach (Match result in searchResults)
        {
            var resultUrl = result.Groups.Values.LastOrDefault()?.Value ?? string.Empty;
            yield return resultUrl;
        }
    }
}