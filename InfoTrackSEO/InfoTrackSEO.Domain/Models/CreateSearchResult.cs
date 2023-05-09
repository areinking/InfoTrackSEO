namespace InfoTrackSEO.Domain.Models
{
    public class CreateSearchResult
    {
       public CreateSearchResult(string searchProvider,
                           DateTime searchDate,
                           string keywords,
                           string targetUrl)
       {
            SearchProvider = searchProvider;
            SearchDate = searchDate;
            Keywords = keywords;
            TargetUrl = targetUrl;
        }

        public string SearchProvider { get; }
        public DateTime SearchDate { get; }
        public string Keywords { get; }
        public string TargetUrl { get; }
    }
}
