using MediatR;

namespace Project1.Commands
{
    public class CustomerScheduledUpdatePostCommandHandler : IRequestHandler<CustomerScheduledUpdateRequestV1Command, CustomerScheduledUpdateRequestV1CommandResult>
    {
        public Task<CustomerScheduledUpdateRequestV1CommandResult> Handle(CustomerScheduledUpdateRequestV1Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
