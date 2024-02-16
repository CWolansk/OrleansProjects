using MediatR;
using DeviceSilo;

namespace Project1.Commands
{
    public class CustomerScheduledUpdatePostCommandHandler : IRequestHandler<CustomerScheduledUpdateRequestV1Command, CustomerScheduledUpdateRequestV1CommandResult>
    {
        private readonly IGrainFactory _grains;

        public CustomerScheduledUpdatePostCommandHandler(IGrainFactory grains)
        {
            _grains = grains;
        }

        public async Task<CustomerScheduledUpdateRequestV1CommandResult> Handle(CustomerScheduledUpdateRequestV1Command request, CancellationToken cancellationToken)
        {
            foreach(var deviceId in request.DeviceIds)
            {
                var grain = _grains.GetGrain<IDeviceGrain>(deviceId);

                await grain.ScheduleUpdate(request.ScheduledStartTime);
            }

            return new CustomerScheduledUpdateRequestV1CommandResult{};
        }
    }
}
