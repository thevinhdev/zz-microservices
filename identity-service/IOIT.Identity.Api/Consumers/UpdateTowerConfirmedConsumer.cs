using IOIT.Identity.Application.Towers.Commands.Update;
using IOIT.Identity.Application.Towers.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class UpdateTowerConfirmedConsumer : IConsumer<DtoCommonTowerUpdatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateTowerConfirmedConsumer> _logger;

        public UpdateTowerConfirmedConsumer(IMediator mediator, ILogger<UpdateTowerConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonTowerUpdatedQueue> context)
        {
            var data = await _mediator.Send(new GetTowerByConditionQuery
            {
                condition = "TowerId=" + context.Message.TowerId +" AND ProjectId="+ context.Message.ProjectId
            });
            if (data != null)
            {
                await _mediator.Send(new UpdateTowerCommand()
                {
                    Id = data.Id,
                    TowerId = context.Message.TowerId,
                    OneSid = context.Message.OneSid,
                    Code = context.Message.Code,
                    Name = context.Message.Name,
                    ProjectId = context.Message.ProjectId,
                    CreatedAt = context.Message.CreatedAt,
                    UpdatedAt = context.Message.UpdatedAt,
                    UpdatedById = context.Message.UserId,
                    Status = (int)context.Message.Status,
                });

                await Task.CompletedTask;
            }
            _logger.LogInformation("Success");
        }
    }
}
