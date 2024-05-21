using IOIT.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IDepartmentAsyncRepository : IAsyncGenericRepository<Department, int>
    {
        Task<Department> FindByDepartmentIdAsync(int? departmentId, CancellationToken cancellationToken);
        Task<Department> GetDataByCondition(string condition);
        Task<List<Department>> GetListDataByCondition(string condition);
    }
}
