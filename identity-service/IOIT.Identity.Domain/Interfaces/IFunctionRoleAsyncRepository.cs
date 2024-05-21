using IOIT.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IFunctionRoleAsyncRepository : IAsyncGenericRepository<FunctionRole, int>
    {
        Task<List<FunctionRole>> GetListFunctionRoleAsync(long targetId, int type, CancellationToken cancellationToken);
        Task<List<FunctionRole>> GetListFunctionRoleByIdAsync(int? functionId, CancellationToken cancellationToken);
        Task<List<FunctionRole>> GetListFunctionRoleUpdateAsync(long targetId, int functionId, int type, CancellationToken cancellationToken);
    }
}
