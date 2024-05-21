using AutoMapper;
using IOIT.Identity.Application.ApartmentMaps.Create;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Consumers
{
    public class CommonApartmentMapConsumer : IConsumer<DtoCommonApartmentMapQueue>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<CommonApartmentMapConsumer> _logger;

        public CommonApartmentMapConsumer(
            IMediator mediator,
            ILogger<CommonApartmentMapConsumer> logger,
            IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<DtoCommonApartmentMapQueue> context)
        {
            _logger.LogInformation($"Received message: {context.Message}");
            var messageDt = context.Message;

            var reqApartment = new CreateApartmentMapCommand()
            {
                Id = messageDt.Id,
                ApartmentId = messageDt.ApartmentId,
                ProjectId = messageDt.ProjectId,
                TowerId = messageDt.TowerId,
                FloorId = messageDt.FloorId,
                ResidentId = messageDt.ResidentId,
                UpdatedAt = messageDt.UpdatedAt,
                UserId = messageDt.UserId,
                Status = messageDt.Status,
            };

            await _mediator.Send(reqApartment);
            _logger.LogInformation("Success");
        }
    }
}
