namespace InfoTrackSEO.Domain.Models
{
    public class SearchResult
    {
        public SearchResult()
        {
            Results = Enumerable.Empty<LinkPosition>();
        }

        public SearchResult Create(CreateSearchResult createSearchResult)
        {
            SearchProvider = createSearchResult.SearchProvider;
            SearchDate = createSearchResult.SearchDate;
            Keywords = Uri.EscapeDataString(createSearchResult.Keywords);
            TargetUrl = createSearchResult.TargetUrl;
            Id = Guid.NewGuid();

            return this;
        }

        public SearchResult SetResults(IEnumerable<LinkPosition> results)
        {
            if (Results.Any())
            {
                throw new ArgumentException("Cannot add more results to an executed search");
            }

            Results = results;

            return this;
        }

        public SearchResult SetDocument(string document)
        {
            if (!string.IsNullOrWhiteSpace(Document))
            {
                throw new ArgumentException("Document has already be set from an executed search");
            }

            Document = Uri.EscapeDataString(document);

            return this;
        }

        public override string ToString()
        {
            if (!Results.Any())
            {
                return "0";
            }

            var hitPositions = Results.Where(r => r.IsHit).Select(r => r.Position).ToList();

            return string.Join(", ", hitPositions);
        }

        public string Position
        {
            get { return this.ToString(); }
        }

        public Guid Id { get; private set; }
        public string? SearchProvider { get; private set; }
        public DateTime SearchDate { get; private set; }
        public string? Keywords { get; private set; }
        public string? TargetUrl { get; private set; }
        public IEnumerable<LinkPosition> Results { get; private set; }
        public string? Document { get; private set; }
    }
}
