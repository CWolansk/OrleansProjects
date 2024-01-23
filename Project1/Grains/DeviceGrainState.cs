using Project1.DomainEvents;

namespace Project1.Grains
{
    [GenerateSerializer]
    public class DeviceGrainState : DomainEventBase
    {
        [Id(0)]
        public DateTimeOffset? ScheduledDateTime { get; set; }

        public DeviceGrainState Apply(DeviceUpdateScheduledEvent @event)
        {
            ScheduledDateTime = @event.scheduledDateTime;
            Version = @event.Version;
            TimeStamp = @event.TimeStamp;
            Id = @event.Id;
            return this;
        }

        public DeviceGrainState Apply(DeviceUpdateScheduleEditedEvent @event)
        {
            ScheduledDateTime = @event.editedScheduledDateTime;
            Version = @event.Version;
            TimeStamp = @event.TimeStamp;
            Id = @event.Id;
            return this;
        }
    }
}
