using IOIT.Identity.Application.Apartments.Commands.Update;
using IOIT.Identity.Application.Apartments.Queries;
using IOIT.Identity.Application.Departments.Commands.Update;
using IOIT.Identity.Application.Departments.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace IOIT.Identity.Api.Consumers
{
    public class UpdateDepartmentConfirmedConsumer : IConsumer<DtoCommonDepartmentUpdatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateDepartmentConfirmedConsumer> _logger;

        public UpdateDepartmentConfirmedConsumer(IMediator mediator, ILogger<UpdateDepartmentConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonDepartmentUpdatedQueue> context)
        {
            var data = await _mediator.Send(new GetDepartmentByConditionQuery
            {
                condition = "DepartmentId=" + context.Message.DepartmentId +" AND ProjectId=" + context.Message.ProjectId
            });
            if (data != null)
            {
                await _mediator.Send(new UpdateDepartmentCommand()
                {
                    Id = data.Id,
                    DepartmentId = context.Message.DepartmentId,
                    Code = context.Message.Code,
                    Name = context.Message.Name,
                    ProjectId = context.Message.ProjectId,
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
