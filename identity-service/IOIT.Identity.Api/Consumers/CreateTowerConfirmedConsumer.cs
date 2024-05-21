using IOIT.Identity.Application.Towers.Commands.Create;
using IOIT.Identity.Application.Towers.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class CreateTowerConfirmedConsumer : IConsumer<DtoCommonTowerCreatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateTowerConfirmedConsumer> _logger;

        public CreateTowerConfirmedConsumer(IMediator mediator, ILogger<CreateTowerConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonTowerCreatedQueue> context)
        {

            await _mediator.Send(new CreateTowerCommand()
            {
                TowerId = context.Message.TowerId,
                OneSid = context.Message.OneSid,
                Code = context.Message.Code,
                Name = context.Message.Name,
                ProjectId = context.Message.ProjectId,
                CreatedAt = context.Message.CreatedAt,
                UpdatedAt = context.Message.UpdatedAt,
                CreatedById = context.Message.UserId,
                UpdatedById = context.Message.UserId,
                Status = context.Message.Status != null ? (int)context.Message.Status : 1,
            }); ;

            await Task.CompletedTask;
            _logger.LogInformation("Success");
        }
    }
}
