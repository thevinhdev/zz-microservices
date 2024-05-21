using IOIT.Identity.Application.Apartments.Commands.Create;
using IOIT.Identity.Application.Floors.Commands.Create;
using IOIT.Identity.Application.Floors.Queries;
using IOIT.Shared.ViewModels;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Api.Consumers
{
    public class CreateFloorConfirmedConsumer : IConsumer<DtoCommonFloorCreatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateFloorConfirmedConsumer> _logger;

        public CreateFloorConfirmedConsumer(IMediator mediator, ILogger<CreateFloorConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonFloorCreatedQueue> context)
        {

            await _mediator.Send(new CreateFloorCommand()
            {
                OneSid = context.Message.TowerId,
                FloorId = context.Message.FloorId,
                TowerId = context.Message.TowerId,
                ProjectId = context.Message.ProjectId,
                Code = context.Message.Code,
                Name = context.Message.Name,
                CreatedAt = context.Message.CreatedAt,
                UpdatedAt = context.Message.UpdatedAt,
                CreatedById = context.Message.UserId,
                UpdatedById = context.Message.UserId,
                Status = context.Message.Status != null ? (int)context.Message.Status : 1,
            });

            await Task.CompletedTask;
            _logger.LogInformation("Success");
        }
    }
}
