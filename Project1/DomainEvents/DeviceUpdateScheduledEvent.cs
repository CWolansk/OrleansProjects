namespace Project1.DomainEvents
{
    public class DeviceUpdateScheduledEvent : DomainEventBase
    {
        public DateTimeOffset scheduledDateTime;
    }
}
