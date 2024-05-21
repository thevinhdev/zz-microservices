using AutoMapper;
using IOIT.Identity.Domain.Entities;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IOIT.Shared.Queues.CommonServiceQueues;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using static IOIT.Shared.Queues.IdentityServiceQueues;

namespace IOIT.Identity.Api.Producers
{
    public class IdentityProducers : IIdentityProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;
        private ILogger<IdentityProducers> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public IdentityProducers(
            ISendEndpointProvider sendEndpointProvider,
            IMapper mapper,
            ILogger<IdentityProducers> logger,
            IPublishEndpoint publishEndpoint)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        #region "Employee"
        public async Task CommonEmployeeAction(Employee data)
        {
            var message = _mapper.Map<DtoCommonEmployeeQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonEmployeeQueue.NameExchange}"));
            await endpoint.Send<List<DtoCommonEmployeeQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonEmployeeQueue.NameExchange} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "Employee Map"
        public async Task CommonEmployeeMapAction(EmployeeMap data)
        {
            var message = _mapper.Map<DtoCommonEmployeeMapQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonEmployeeMapQueue.NameExchange}"));
            await endpoint.Send<List<DtoCommonEmployeeMapQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonEmployeeMapQueue.NameExchange} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        public async Task CommonEmployeeMapUpdate(EmployeeMap data)
        {
            var message = _mapper.Map<DtoCommonEmployeeMapUpdatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonEmployeeMapUpdatedQueue.NameExchange}"));
            await endpoint.Send<List<DtoCommonEmployeeMapUpdatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonEmployeeMapUpdatedQueue.NameExchange} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "User"
        public async Task CommonUserAction(User data)
        {
            var message = _mapper.Map<DtoCommonUserQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonUserQueue.NameExchange}"));
            await endpoint.Send<List<DtoCommonUserQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonUserQueue.NameExchange} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "Resident"
        public async Task CommonResidentAction(Resident data)
        {
            var message = _mapper.Map<DtoCommonResidentQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonResidentQueue.NameExchange}"));
            await endpoint.Send<List<DtoCommonResidentQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonResidentQueue.NameExchange} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "Apartmap Map"
        public async Task IdentityApartmentMapCreate(ApartmentMap data)
        {
            var messageA = _mapper.Map<DtoIdentityApartmentMapQueue>(data);
            await _publishEndpoint.Publish<DtoIdentityApartmentMapQueue>(messageA);

            _logger.LogInformation($"Sent message to queues {IdentityApartmentMapQueue.NameExchange} success with payload: {JsonConvert.SerializeObject(messageA)}");
        }

        #endregion
    }
}
