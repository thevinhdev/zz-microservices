using IOIT.Identity.Application.Towers.Commands.Create;
using IOIT.Identity.Application.TypeAttributeItems.Commands.Create;
using IOIT.Identity.Application.TypeAttributeItems.Queries;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class CreateTypeAttributeItemConfirmedConsumer : IConsumer<DtoCommonTypeAttributeItemCreatedQueue>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateTypeAttributeItemConfirmedConsumer> _logger;

        public CreateTypeAttributeItemConfirmedConsumer(IMediator mediator, ILogger<CreateTypeAttributeItemConfirmedConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DtoCommonTypeAttributeItemCreatedQueue> context)
        {
            await _mediator.Send(new CreateTypeAttributeItemCommand()
            {
                TypeAttributeItemId = context.Message.TypeAttributeItemId,
                Code = context.Message.Code,
                Name = context.Message.Name,
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
