namespace InfoTrackSEO.Domain.EventBus
{
    public class EventBus : IEventBus
    {
        public async Task AsyncPublish(IDomainEvent domainEvent)
        {
            await Task.Run(() =>
            { 
                // This is where we would actually publish an event,
                // but I'm not going that far in this demo
                Console.WriteLine(domainEvent);
            });
        }
    }
}
