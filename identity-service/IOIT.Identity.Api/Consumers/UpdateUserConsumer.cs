

using AutoMapper;
using IOIT.Identity.Application.Users.Commands.Update;
using IOIT.Identity.Domain.Entities.Indentity;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static IOIT.Identity.Api.Controllers.AuthController;

namespace IOIT.Identity.Api.Consumers
{
    public class UpdateUserConsumer : IConsumer<CustomerAccountClosed>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserConsumer> _logger;

        public UpdateUserConsumer(IMediator mediator, IMapper mapper, ILogger<UpdateUserConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Consume(ConsumeContext<CustomerAccountClosed> context)
        {
            _logger.LogInformation("Revceived messages:", context.Message);
            //var command = _mapper.Map<UpdateUserByIdCommand>(context.Message);
            //await _mediator.Send(command);

            _logger.LogInformation("Success");

            return Task.CompletedTask;
        }
    }
}
