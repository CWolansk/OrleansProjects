namespace Project1.Grains
{
    [GenerateSerializer]
    public class DeviceGrainState
    {
        [Id(0)]
        public DateTime? ScheduledDateTime { get; set; }
    }
}
