using MediatR;

namespace Project1.Commands
{
    public class CustomerScheduledUpdateReviseV1Command : IRequest<CustomerScheduledUpdateReviseV1CommandResult>
    {
        public string DeviceId { get; set; }

        public DateTime RevisedScheduledStartTime { get; set; }
    }
}
