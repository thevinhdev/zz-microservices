using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class FunctionRoleAsyncRepository : AsyncGenericRepository<FunctionRole, int>, IFunctionRoleAsyncRepository
    {
        public FunctionRoleAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<FunctionRole>> GetListFunctionRoleAsync(long targetId, int type, CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking().Where(c => c.TargetId == targetId && c.Type == type && c.Status == AppEnum.EntityStatus.NORMAL).ToListAsync();
        }

        public async Task<List<FunctionRole>> GetListFunctionRoleByIdAsync(int? functionId, CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking().Where(c => c.FunctionId == functionId && c.Status == AppEnum.EntityStatus.NORMAL).ToListAsync();
        }

        public async Task<List<FunctionRole>> GetListFunctionRoleUpdateAsync(long targetId, int functionId, int type, CancellationToken cancellationToken)
        {
            return await DbSet.Where(e => e.TargetId == targetId
                        && e.FunctionId == functionId && e.Type == type
                        && e.Status != AppEnum.EntityStatus.DELETED).ToListAsync();
        }

    }
}
