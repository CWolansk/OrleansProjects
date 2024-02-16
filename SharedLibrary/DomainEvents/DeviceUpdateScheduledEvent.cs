namespace DomainEvents
{
    [GenerateSerializer]
    public class DeviceUpdateScheduledEvent : DomainEventBase
    {
        public DateTimeOffset scheduledDateTime;
    }
}
