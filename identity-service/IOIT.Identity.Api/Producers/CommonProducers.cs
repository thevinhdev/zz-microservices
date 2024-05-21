using AutoMapper;
using IOIT.Common.Application.Apartments.ViewModels;
using IOIT.Common.Application.Common.Interfaces.Producer;
using IOIT.Common.Application.Floors.ViewModels;
using IOIT.Common.Application.Projects.ViewModels;
using IOIT.Common.Application.Towers.ViewModels;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IOIT.Shared.Queues.CommonServiceQueues;

namespace IOIT.Common.Api.Producers
{
    public class CommonProducers : ICommonProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;
        private ILogger<CommonProducers> _logger;

        public CommonProducers(
            ISendEndpointProvider sendEndpointProvider,
            IMapper mapper,
            ILogger<CommonProducers> logger)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _mapper = mapper;
            _logger = logger;
        }

        #region "Project"
        public async Task CommonProjectCreated(ResGetProjectById data)
        {
            var message = _mapper.Map<DtoCommonProjectCreatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonProjectCreatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonProjectCreatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonProjectCreatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        public async Task CommonProjectUpdated(ResGetProjectById data)
        {
            var message = _mapper.Map<DtoCommonProjectUpdatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonProjectUpdatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonProjectUpdatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonProjectUpdatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "Tower"
        public async Task CommonTowerCreated(ResGetTowerById data)
        {
            var message = _mapper.Map<DtoCommonProjectCreatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonTowerCreatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonTowerCreatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonTowerCreatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        public async Task CommonTowerUpdated(ResGetTowerById data)
        {
            var message = _mapper.Map<DtoCommonProjectUpdatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonTowerUpdatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonTowerUpdatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonTowerUpdatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "Floor"
        public async Task CommonFloorCreated(ResGetFloorById data)
        {
            var message = _mapper.Map<DtoCommonFloorCreatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonFloorCreatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonFloorCreatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonFloorCreatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        public async Task CommonFloorUpdated(ResGetFloorById data)
        {
            var message = _mapper.Map<DtoCommonFloorUpdatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonFloorUpdatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonFloorUpdatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonFloorUpdatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

        #region "Apartment"
        public async Task CommonApartmentCreated(ResGetApartmentById data)
        {
            var message = _mapper.Map<DtoCommonApartmentCreatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonApartmentCreatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonApartmentCreatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonApartmentCreatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        public async Task CommonApartmentUpdated(ResGetApartmentById data)
        {
            var message = _mapper.Map<DtoCommonApartmentUpdatedQueue>(data);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{CommonApartmentUpdatedQueue.Name}"));
            await endpoint.Send<List<DtoCommonApartmentUpdatedQueue>>(message);
            _logger.LogInformation($"Sent message to queues {CommonApartmentUpdatedQueue.Name} success with payload: {JsonConvert.SerializeObject(message)}");
        }

        #endregion

    }
}
