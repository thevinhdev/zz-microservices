using IOIT.Identity.Application.Apartments.Commands.Update;
using IOIT.Identity.Application.Apartments.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace IOIT.Identity.Api.Consumers
{
    public class UpdateApartmentConfirmedConsumer : IConsumer<DtoCommonApartmentUpdatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateApartmentConfirmedConsumer> _logger;

        public UpdateApartmentConfirmedConsumer(IMediator mediator, ILogger<UpdateApartmentConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonApartmentUpdatedQueue> context)
        {
            var data = await _mediator.Send(new GetApartmentByConditionQuery
            {
                condition = "ApartmentId=" + context.Message.ApartmentId + " AND FloorId=" + context.Message.FloorId + " AND TowerId=" + context.Message.TowerId + " AND ProjectId=" + context.Message.ProjectId
            });
            if (data != null)
            {
                await _mediator.Send(new UpdateApartmentCommand()
                {
                    Id = data.Id,
                    ApartmentId = context.Message.ApartmentId,
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
