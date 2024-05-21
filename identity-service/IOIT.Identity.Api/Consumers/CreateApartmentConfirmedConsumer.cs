using IOIT.Identity.Application.Apartments.Commands.Create;
using IOIT.Identity.Application.Apartments.Queries;
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
    public class CreateApartmentConfirmedConsumer : IConsumer<DtoCommonApartmentCreatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateApartmentConfirmedConsumer> _logger;

        public CreateApartmentConfirmedConsumer(IMediator mediator, ILogger<CreateApartmentConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonApartmentCreatedQueue> context)
        {
            //var data = await _mediator.Send(new GetApartmentByConditionQuery
            //{
            //    condition = "ApartmentId=" + context.Message.ApartmentId
            //});
            //if (data != null)
            //{
                await _mediator.Send(new CreateApartmentCommand()
                {
                    ApartmentId = context.Message.ApartmentId,
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
            //}
            _logger.LogInformation("Success");
        }
    }
}
