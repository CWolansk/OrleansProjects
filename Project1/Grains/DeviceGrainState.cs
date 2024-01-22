using Project1.DomainEvents;

namespace Project1.Grains
{
    [GenerateSerializer]
    public class DeviceGrainState
    {
        public DateTimeOffset? ScheduledDateTime { get; set; }

        public DeviceGrainState Apply(DeviceUpdateScheduledEvent @event)
        {
            ScheduledDateTime = @event.scheduledDateTime;   
            return this;
        }

        public DeviceGrainState Apply(DeviceUpdateScheduleEditedEvent @event)
        {
            ScheduledDateTime = @event.editedScheduledDateTime;
            return this;
        }
    }
}
