using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class RoleAsyncRepository : AsyncGenericRepository<Role, int>, IRoleAsyncRepository
    {
        public RoleAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Role> FindByCodeAsync(string code, int roleId,CancellationToken cancellationToken)
        {
            return await DbSet.Where(e=>e.Code == code && e.Id != roleId && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
        }
    }
}
