namespace Project1.DomainEvents
{
    [GenerateSerializer]
    public class DeviceUpdateScheduledEvent : DomainEventBase
    {
        public DateTimeOffset scheduledDateTime;
    }
}
