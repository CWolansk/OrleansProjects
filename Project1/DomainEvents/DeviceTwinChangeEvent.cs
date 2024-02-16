using Orleans.Streams;

namespace Project1.DomainEvents
{
    [GenerateSerializer]
    public class DeviceTwinChangeEvent : DomainEventBase
    {
        public string? Payload { get; set; }

        public StreamSequenceToken? CurrentStreamSequenceToken { get; set; }

    }
}
