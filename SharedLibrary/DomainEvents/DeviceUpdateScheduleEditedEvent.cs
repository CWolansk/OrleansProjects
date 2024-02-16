namespace DomainEvents
{
    [GenerateSerializer]
    public class DeviceUpdateScheduleEditedEvent : DomainEventBase
    {
        public DateTimeOffset editedScheduledDateTime;
    }
}
