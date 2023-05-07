namespace InfoTrackSEO.Domain.Models
{
    public class SearchResult
    {
        public string? Keywords { get; set; }
        public string? Url { get; set; }
        public string? SearchEngine { get; set; }
        public string? Positions { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
