using MediatR;

namespace Project1.Commands
{
    public class CustomerScheduledUpdatePatchCommandHandler : IRequestHandler<CustomerScheduledUpdateReviseV1Command, CustomerScheduledUpdateReviseV1CommandResult>
    {
        public Task<CustomerScheduledUpdateReviseV1CommandResult> Handle(CustomerScheduledUpdateReviseV1Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
