namespace InfoTrackSEO.Domain.EventBus
{
    public interface IEventBus
    {
        Task AsyncPublish(IDomainEvent domainEvent);
    }
}
