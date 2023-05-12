using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Models;

public class CreateSearchResultDomainEvent : IDomainEvent
{
    public SearchResult? SearchResult { get; set; }
}