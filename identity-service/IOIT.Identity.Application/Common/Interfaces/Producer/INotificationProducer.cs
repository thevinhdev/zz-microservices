using IOIT.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Common.Interfaces.Producer
{
    public interface INotificationProducer
    {
        Task NotificationActionCreate(List<DtoNotificationActionCreateQueue> message);
    }
}
