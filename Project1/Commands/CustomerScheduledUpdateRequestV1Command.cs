using MediatR;

namespace Project1.Commands
{
    public class CustomerScheduledUpdateRequestV1Command : IRequest<CustomerScheduledUpdateRequestV1CommandResult>
    {
        public List<string> DeviceIds { get; set; }

        public DateTime ScheduledStartTime { get; set; }
    }
}
