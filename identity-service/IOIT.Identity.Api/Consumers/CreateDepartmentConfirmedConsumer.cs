using IOIT.Identity.Application.Apartments.Commands.Create;
using IOIT.Identity.Application.Departments.Commands.Create;
using IOIT.Identity.Application.Departments.Queries;
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
    public class CreateDepartmentConfirmedConsumer : IConsumer<DtoCommonDepartmentCreatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateDepartmentConfirmedConsumer> _logger;

        public CreateDepartmentConfirmedConsumer(IMediator mediator, ILogger<CreateDepartmentConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonDepartmentCreatedQueue> context)
        {

            await _mediator.Send(new CreateDepartmentCommand()
            {
                DepartmentId = context.Message.DepartmentId,
                Code = context.Message.Code,
                Name = context.Message.Name,
                ProjectId = context.Message.ProjectId,
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
