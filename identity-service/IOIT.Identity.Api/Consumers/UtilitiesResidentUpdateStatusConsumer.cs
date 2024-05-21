using IOIT.Identity.Application.Residents.Commands.Update;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class UtilitiesResidentUpdateStatusConsumer : IConsumer<DtoUtilitiesResidentUpdateStatusQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UtilitiesResidentUpdateConsumer> _logger;

        public UtilitiesResidentUpdateStatusConsumer(IMediator mediator, ILogger<UtilitiesResidentUpdateConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoUtilitiesResidentUpdateStatusQueue> context)
        {
            _logger.LogInformation($"Received message: {context.Message}");

            await _mediator.Send(new UpdateStatusResidentCommand()
            {
                Id = context.Message.ResidentId,
                UserId = context.Message.UserId,
                Status = context.Message.Status
            });

            await Task.CompletedTask;

            _logger.LogInformation("Success");
        }
    }
}
