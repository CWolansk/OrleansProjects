namespace Project1.DomainEvents
{
    public class DeviceUpdateScheduleEditedEvent : DomainEventBase
    {
        public DateTimeOffset editedScheduledDateTime;
    }
}
