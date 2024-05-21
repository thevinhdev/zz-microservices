using IOIT.Identity.Application.Projects.Commands.Update;
using IOIT.Identity.Application.Projects.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class UpdateProjectConfirmedConsumer : IConsumer<DtoCommonProjectUpdatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateProjectConfirmedConsumer> _logger;

        public UpdateProjectConfirmedConsumer(IMediator mediator, ILogger<UpdateProjectConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonProjectUpdatedQueue> context)
        {
            var data = await _mediator.Send(new GetProjectByConditionQuery
            {
                condition = "ProjectId=" + context.Message.ProjectId
            });

            if (data != null)
            {
                await _mediator.Send(new UpdateProjectCommand()
                {
                    Id = data.Id,
                    ProjectId = context.Message.ProjectId,
                    OneSid = context.Message.OneSId,
                    Code = context.Message.Code,
                    Name = context.Message.Name,
                    UpdatedAt = context.Message.UpdatedAt,
                    UpdatedById = context.Message.UpdatedById,
                    Status = (int)context.Message.Status,
                });

                await Task.CompletedTask;
            }
            _logger.LogInformation("Success");
        }
    }
}
