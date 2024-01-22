namespace Project1.DomainEvents
{
    [GenerateSerializer]
    public class DeviceUpdateScheduleEditedEvent : DomainEventBase
    {
        public DateTimeOffset editedScheduledDateTime;
    }
}
