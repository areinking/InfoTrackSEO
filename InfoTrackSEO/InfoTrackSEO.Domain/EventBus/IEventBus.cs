namespace InfoTrackSEO.Domain.EventBus
{
    public interface IEventBus
    {
        // this bus would allow other microservices to kick off based upon domain changes
        Task AsyncPublish(IDomainEvent domainEvent);
    }
}
