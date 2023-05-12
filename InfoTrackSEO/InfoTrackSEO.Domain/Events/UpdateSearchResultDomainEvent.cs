using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Models;

public class UpdateSearchResultDomainEvent : IDomainEvent
{
    public SearchResult? SearchResult { get; set; }
}