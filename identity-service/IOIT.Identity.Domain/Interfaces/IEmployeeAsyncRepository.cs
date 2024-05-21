using IOIT.Identity.Domain.Entities;
using IOIT.Shared.ViewModels.PagingQuery;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IEmployeeAsyncRepository : IAsyncGenericRepository<Employee, int>
    {
        Task<IList<Employee>> FindByPositionAsync(int positionId, CancellationToken cancellationToken);
        Task<IPagedResult<Employee>> GetByPageHandelAsync(FilterPagination paging, int? towerId, int? departmentId, long? userId, CancellationToken cancellationToken);
        Task<Employee> FindByUserMapIdAsync(long? userMapId, CancellationToken cancellationToken);
        Task<Employee> FindByCodeAsync(string code, int type, CancellationToken cancellationToken);
        Task<Employee> FindByPhoneAsync(string phone, int type, CancellationToken cancellationToken);
    }
}
