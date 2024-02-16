namespace DomainEvents
{
    [GenerateSerializer]
    public class DomainEventBase
    {
        [Id(0)]
        public string Id { get; set; }

        [Id(1)]
        public int Version { get; set; } = 0;

        [Id(2)]
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.Now;
    }
}
