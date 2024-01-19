namespace Project1.DomainEvents
{
    public class DeviceUpdateScheduled : DomainEventBase
    {
        public DateTimeOffset scheduledDateTime;
    }
}
