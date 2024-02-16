using MediatR;
using DeviceSilo;

namespace Project1.Commands
{
    public class CustomerScheduledUpdatePatchCommandHandler : IRequestHandler<CustomerScheduledUpdateReviseV1Command, CustomerScheduledUpdateReviseV1CommandResult>
    {
        private readonly IGrainFactory _grains;

        public CustomerScheduledUpdatePatchCommandHandler(IGrainFactory grains)
        {
            _grains = grains;
        }

        public async Task<CustomerScheduledUpdateReviseV1CommandResult> Handle(CustomerScheduledUpdateReviseV1Command request, CancellationToken cancellationToken)
        {
            var grain = _grains.GetGrain<IDeviceGrain>(request.DeviceId);

            await grain.EditScheduledUpdate(request.RevisedScheduledStartTime);

            var state = await grain.GetState();

            return new CustomerScheduledUpdateReviseV1CommandResult
            {
                DeviceGrain = new DeviceGrainState
                {
                    ScheduledDateTime = state.ScheduledDateTime
                }
            };
        }
    }
}
