using Orleans.Streams;
using DomainEvents;

namespace DeviceSilo
{
    [GenerateSerializer]
    public class DeviceGrainState : DomainEventBase
    {
        [Id(0)]
        public DateTimeOffset? ScheduledDateTime { get; set; }

        [Id(1)]
        public StreamSequenceToken? CurrentStreamSequenceToken { get; set; }

        [Id(2)]
        public string? Payload { get; set; }

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

        public DeviceGrainState Apply(DeviceTwinChangeEvent @event)
        {
            Payload = @event.Payload;
            CurrentStreamSequenceToken = @event.CurrentStreamSequenceToken;
            Version = @event.Version;
            Version = @event.Version;
            TimeStamp = @event.TimeStamp;
            Id = @event.Id;
            return this;
        }
    }
}
