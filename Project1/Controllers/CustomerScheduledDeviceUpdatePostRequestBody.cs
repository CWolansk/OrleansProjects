namespace Project1.Controllers
{
    public class CustomerScheduledDeviceUpdatePostRequestBody
    {
        public string DeviceId { get; set; }

        public DateTime ScheduledStartTime { get; set; }
    }
}
