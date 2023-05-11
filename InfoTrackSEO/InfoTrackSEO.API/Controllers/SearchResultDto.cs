namespace InfoTrackSEO.API.Controllers
{
    public class SearchResultDto
    {
        public string? Keywords { get; set; }
        public string? URL { get; set; }
        public string? Positions { get; set; }
        public string? SearchDate { get; set; }
        public string? SearchEngine { get; set; }
        public IEnumerable<Result>? Results { get; set; }
    }
}
