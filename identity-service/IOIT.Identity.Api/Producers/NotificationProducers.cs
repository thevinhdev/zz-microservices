using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Shared.ViewModels;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IOIT.Shared.Queues.NotificationServiceQueues;

namespace IOIT.Identity.Api.Producers
{
    public class NotificationProducers : INotificationProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public NotificationProducers(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task NotificationActionCreate(List<DtoNotificationActionCreateQueue> messages)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{NotificationActionCreateQueue.Name}"));
            foreach (var message in messages)
            {
                await endpoint.Send<DtoNotificationActionCreateQueue>(message);
            }
        }
    }
}
