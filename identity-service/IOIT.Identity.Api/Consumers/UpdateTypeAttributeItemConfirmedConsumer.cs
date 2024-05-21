using IOIT.Identity.Application.Towers.Commands.Update;
using IOIT.Identity.Application.Towers.Queries;
using IOIT.Identity.Application.TypeAttributeItems.Commands.Update;
using IOIT.Identity.Application.TypeAttributeItems.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class UpdateTypeAttributeItemConfirmedConsumer : IConsumer<DtoCommonTypeAttributeItemUpdatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateTypeAttributeItemConfirmedConsumer> _logger;

        public UpdateTypeAttributeItemConfirmedConsumer(IMediator mediator, ILogger<UpdateTypeAttributeItemConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonTypeAttributeItemUpdatedQueue> context)
        {
            var data = await _mediator.Send(new GetTypeAttributeItemByConditionQuery
            {
                condition = "TypeAttributeItemId=" + context.Message.TypeAttributeItemId
            });
            if (data != null)
            {
                await _mediator.Send(new UpdateTypeAttributeItemCommand()
                {
                    Id = data.Id,
                    TypeAttributeItemId = context.Message.TypeAttributeItemId,
                    Code = context.Message.Code,
                    Name = context.Message.Name,
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
