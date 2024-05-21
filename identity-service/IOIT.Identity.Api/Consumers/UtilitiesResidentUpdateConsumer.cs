using AutoMapper;
using IOIT.Identity.Application.Residents.Commands.Update;
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
    public class UtilitiesResidentUpdateConsumer : IConsumer<DtoUtitlitiesResidentUpdateQueue>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UtilitiesResidentUpdateConsumer> _logger;

        public UtilitiesResidentUpdateConsumer(
            IMediator mediator, 
            ILogger<UtilitiesResidentUpdateConsumer> logger,
            IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<DtoUtitlitiesResidentUpdateQueue> context)
        {
            _logger.LogInformation($"Received message: {context.Message}");

            //var command = _mapper.Map<UpdateResidentCommand>(context.Message);

            var lstApartmentMap = new List<Domain.ViewModels.ApartmentMapDT>();

            if (context.Message.apartments.Count > 0)
            {
                foreach(var item in context.Message.apartments)
                {
                    lstApartmentMap.Add(new Domain.ViewModels.ApartmentMapDT()
                    {
                        ApartmentId = item.ApartmentId,
                        FloorId = item.FloorId,
                        TowerId = item.TowerId,
                        ProjectId = item.ProjectId,
                        Type = item.Type,
                        RelationshipId = item.RelationshipId,
                        DateRent = item.DateRent,
                        DateStart = item.DateStart,
                        DateEnd = item.DateEnd
                    });
                }
            }

            await _mediator.Send(new UpdateResidentCommand() { 
                ResidentId = context.Message.ResidentId,
                Id = context.Message.ResidentId,
                ProjectId = context.Message.ProjectId,
                ResidentParentId = context.Message.ResidentParentId,
                FullName = context.Message.FullName,
                Birthday = context.Message.Birthday,
                CardId = context.Message.CardId,
                DateId = context.Message.DateId,
                AddressId = context.Message.AddressId,
                Phone = context.Message.Phone,
                Email = context.Message.Email,
                Address = context.Message.Address,
                Avata = context.Message.Avata,
                Sex = context.Message.Sex,
                Note = context.Message.Note,
                DateRent = context.Message.DateRent,
                UserId = context.Message.UserId,
                CountryId = context.Message.CountryId,
                Status = (byte?)context.Message.Status,
                apartments = lstApartmentMap,
                TypeCardId = (Domain.Enum.DomainEnum.ResidentRequestIdentifyType?)context.Message.TypeCardId

            });

            await Task.CompletedTask;

            _logger.LogInformation("Success");
        }
    }
}
