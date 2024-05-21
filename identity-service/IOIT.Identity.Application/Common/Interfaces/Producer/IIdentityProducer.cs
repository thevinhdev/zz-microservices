using IOIT.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Common.Interfaces.Producer
{
    public interface IIdentityProducer
    {
        Task CommonEmployeeAction(Employee data);
        Task CommonEmployeeMapAction(EmployeeMap data);
        Task CommonEmployeeMapUpdate(EmployeeMap data);
        Task CommonUserAction(User data);
        Task CommonResidentAction(Resident data);
        Task IdentityApartmentMapCreate(ApartmentMap data);
    }
}
