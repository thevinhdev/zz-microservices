using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IUserRoleAsyncRepository : IAsyncGenericRepository<UserRole, int>
    {
        Task<List<UserRole>> GetListDataByCondition(string condition);
        Task<List<UserRole>> GetListUserRoleAsync(long userId, CancellationToken cancellationToken);
        Task<IList<UserRole>> GetListUserRoleNewAsync(long userId, int roleId, CancellationToken cancellationToken);
        Task<List<RoleDT>> GetListRoleAsync(long userId, CancellationToken cancellationToken);
    }
}
