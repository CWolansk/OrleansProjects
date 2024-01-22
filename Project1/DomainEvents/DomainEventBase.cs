namespace Project1.DomainEvents
{
    [GenerateSerializer]
    public class DomainEventBase
    {
        public string DeviceId { get; set; }

        public int Version { get; set; } = 0;
        
        public DateTimeOffset TimeStamp = DateTimeOffset.Now;
    }
}
