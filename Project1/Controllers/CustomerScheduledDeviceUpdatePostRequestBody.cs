namespace Project1.Controllers
{
    public class CustomerScheduledDeviceUpdatePostRequestBody
    {
        public List<string> DeviceIds { get; set; }

        public DateTime ScheduledStartTime { get; set; }
    }
}
