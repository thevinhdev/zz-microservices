using IOIT.Identity.Application.Projects.Commands.Create;
using IOIT.Identity.Application.Projects.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Api.Consumers
{
    public class CreateProjectConfirmedConsumer : IConsumer<DtoCommonProjectCreatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateProjectConfirmedConsumer> _logger;

        public CreateProjectConfirmedConsumer(IMediator mediator, ILogger<CreateProjectConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonProjectCreatedQueue> context)
        {

            await _mediator.Send(new CreateProjectCommand()
            {
                ProjectId = context.Message.ProjectId,
                OneSid = context.Message.OneSId,
                Code = context.Message.Code,
                Name = context.Message.Name,
                CreatedAt = context.Message.CreatedAt,
                UpdatedAt = context.Message.UpdatedAt,
                CreatedById = context.Message.UserId,
                UpdatedById = context.Message.UserId,
                Status = (int)context.Message.Status,
            });

            await Task.CompletedTask;

            _logger.LogInformation("Success");
        }
    }
}
