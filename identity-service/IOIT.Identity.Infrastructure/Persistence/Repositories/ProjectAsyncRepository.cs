using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class ProjectAsyncRepository : AsyncGenericRepository<Project, int>, IProjectAsyncRepository
    {
        public ProjectAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Project> FindByProjectIdAsync(int? projectId, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.ProjectId == projectId, cancellationToken);
        }

        public async Task<Project> GetDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.AsNoTracking().Where(condition).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<List<Project>> GetListDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.Where(condition).ToListAsync();

            return entity;
        }
    }
}
