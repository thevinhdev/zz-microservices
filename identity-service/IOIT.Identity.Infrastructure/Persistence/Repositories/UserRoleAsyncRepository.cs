using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class UserRoleAsyncRepository : AsyncGenericRepository<UserRole, int>, IUserRoleAsyncRepository
    {
        public UserRoleAsyncRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<List<UserRole>> GetListDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.Where(condition).ToListAsync();

            return entity;
        }

        public async Task<List<UserRole>> GetListUserRoleAsync(long userId, CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking().Where(c => c.UserId == userId && c.Status != AppEnum.EntityStatus.DELETED).ToListAsync();
        }

        public async Task<IList<UserRole>> GetListUserRoleNewAsync(long userId, int roleId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(c => c.UserId == userId && c.RoleId == roleId && c.Status != AppEnum.EntityStatus.DELETED).ToListAsync();
        }

        async Task<List<RoleDT>> IUserRoleAsyncRepository.GetListRoleAsync(long userId, CancellationToken cancellationToken)
        {
            return await (from ur in DbSet
                          join r in DbContext.Set<Role>() on ur.RoleId equals r.Id
                          where ur.UserId == userId && ur.Status != AppEnum.EntityStatus.DELETED
                          select new RoleDT
                          {
                              RoleId = r.Id,
                              RoleName = r.Name,
                          }).ToListAsync();
        }

    }
}
