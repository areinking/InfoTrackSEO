using System.Text.RegularExpressions;

namespace InfoTrackSEO.Domain.DomainServices
{
    public abstract class BaseSearchService
    {
        protected async Task<string> FetchSearchResultsAsync(string searchUrl)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(searchUrl);
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        protected IEnumerable<int> GetPositions(string document, string targetUrl)
        {
            var hyperlinkRegex = new Regex("<a.*href=.*>", RegexOptions.IgnoreCase);
            var hrefRegex = new Regex("href=[\"\']?([^ >\"\']*)", RegexOptions.IgnoreCase);
            var searchResults = hyperlinkRegex.Matches(document);
            var positions = new List<int>();
            int position = 1;

            foreach (Match result in searchResults)
            {
                if (position > 100){
                    break;
                }
				
				var link = result.Value;

                var resultUrl = hrefRegex.Match(link).Groups.Values.FirstOrDefault()?.Value ?? string.Empty;

                if (resultUrl.Contains(targetUrl))
                {
                    positions.Add(position);
                }

                position++;
            }

            if (!positions.Any()) {
                positions.Add(0);
            }

            return positions;
        }
    }
}
