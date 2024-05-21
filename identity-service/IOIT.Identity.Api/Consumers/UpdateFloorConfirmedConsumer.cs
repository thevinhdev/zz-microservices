using IOIT.Identity.Application.Apartments.Commands.Update;
using IOIT.Identity.Application.Apartments.Queries;
using IOIT.Identity.Application.Floors.Commands.Update;
using IOIT.Identity.Application.Floors.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace IOIT.Identity.Api.Consumers
{
    public class UpdateFloorConfirmedConsumer : IConsumer<DtoCommonFloorUpdatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateFloorConfirmedConsumer> _logger;

        public UpdateFloorConfirmedConsumer(IMediator mediator, ILogger<UpdateFloorConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonFloorUpdatedQueue> context)
        {
            var data = await _mediator.Send(new GetFloorByConditionQuery
            {
                condition = "FloorId=" + context.Message.FloorId + " AND TowerId=" + context.Message.TowerId + " AND ProjectId=" + context.Message.ProjectId
            });
            if (data != null)
            {
                await _mediator.Send(new UpdateFloorCommand()
                {
                    Id = data.Id,
                    OneSid = context.Message.TowerId,
                    FloorId = context.Message.FloorId,
                    TowerId = context.Message.TowerId,
                    ProjectId = context.Message.ProjectId,
                    Code = context.Message.Code,
                    Name = context.Message.Name,
                    UpdatedAt = context.Message.UpdatedAt,
                    UpdatedById = context.Message.UserId,
                    Status = context.Message.Status != null ? (int)context.Message.Status : 1,
                });

                await Task.CompletedTask;
            }
            _logger.LogInformation("Success");
        }
    }
}
