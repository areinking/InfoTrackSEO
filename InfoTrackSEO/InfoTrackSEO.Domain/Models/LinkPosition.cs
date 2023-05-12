using System.Text.Json.Serialization;
using InfoTrackSEO.Domain.Models;

public class LinkPosition
{
    private Guid? _searchResultId;

    public LinkPosition()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public int Position { get; set; }
    public string? Url { get; set; }
    public bool IsHit { get; set; }

    [JsonIgnore]
    public virtual SearchResult? SearchResult { get; set; }
    public Guid? SearchResultId
    {
        get => _searchResultId;
        set => _searchResultId = value;
    }
}
