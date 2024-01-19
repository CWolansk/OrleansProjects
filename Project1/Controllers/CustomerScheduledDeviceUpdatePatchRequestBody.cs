namespace Project1.Controllers
{
    public class CustomerScheduledDeviceUpdatePatchRequestBody
    {
        public string DeviceId { get; set; }

        public DateTime RevisedScheduledStartTime { get; set; }
    }
}
