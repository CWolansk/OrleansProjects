using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orleans.Runtime;
using Project1.Commands;
using Project1.Grains;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerUpdateController
    {
        private readonly IMediator _mediator;
        private readonly IGrainFactory _grainFacotry;

        public CustomerUpdateController(IMediator mediator, IGrainFactory grainFacotry)
        {
            _grainFacotry = grainFacotry;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Events")]
        public async Task<IResult> GetEvents(string deviceId)
        {
            var grain = _grainFacotry.GetGrain<IDeviceGrain>(deviceId);

            return Results.Ok(grain.GetEvents());
        }

        [HttpGet]
        [Route("State")]
        public async Task<IResult> GetState(string deviceId)
        {
            var grain = _grainFacotry.GetGrain<IDeviceGrain>(deviceId);

            return Results.Ok(grain.GetState());
        }

        [HttpGet]
        public async Task<IResult> GetScheduleDetails(string deviceId, CancellationToken cancellationToken)
        {
            //Query

            var result = await _mediator.Send(deviceId, cancellationToken);

            return Results.Ok(result);
        }

        [HttpPost]
        public async Task<IResult> ScheduleUpdate([FromBody] CustomerScheduledDeviceUpdatePostRequestBody request, CancellationToken cancellationToken)
        {
            //Command
            var command = new CustomerScheduledUpdateRequestV1Command
            {
                DeviceIds = request.DeviceIds,
                ScheduledStartTime = request.ScheduledStartTime,
            };

            var commandResult = await _mediator.Send(command, cancellationToken);

            return Results.Ok(commandResult);
        }

        [HttpPatch]
        public async Task<IResult> EditScheduledUpdate([FromBody] CustomerScheduledDeviceUpdatePatchRequestBody request, CancellationToken cancellationToken)
        {
            //Command
            var command = new CustomerScheduledUpdateReviseV1Command
            {
                DeviceId = request.DeviceId,
                RevisedScheduledStartTime = request.RevisedScheduledStartTime,
            };
        
            var commandResult = await _mediator.Send(command, cancellationToken);

            return Results.Ok(commandResult);
        }
    }
}
