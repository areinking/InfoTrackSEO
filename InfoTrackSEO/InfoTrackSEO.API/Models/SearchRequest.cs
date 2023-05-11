public class SearchRequest
{
    public string? Url { get; set; }
    public string? Keywords { get; set; }
    public string? SearchEngine { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(this.Keywords) &&
               !string.IsNullOrWhiteSpace(this.SearchEngine) &&
               !string.IsNullOrWhiteSpace(this.Url);
    }
}