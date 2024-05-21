using IOIT.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IEmployeeMapAsyncRepository : IAsyncGenericRepository<EmployeeMap, int>
    {
        Task<EmployeeMap> GetEmployeeTowerAsync(int employeeId, int towerId, CancellationToken cancellationToken);
        Task<List<EmployeeMap>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
    }
}
