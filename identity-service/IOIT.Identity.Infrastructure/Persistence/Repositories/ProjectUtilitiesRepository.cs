using IOIT.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOIT.Identity.Domain.Entities;
using IOIT.Shared.Commons.Enum;
using Microsoft.EntityFrameworkCore;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class ProjectUtilitiesRepository : AsyncGenericRepository<ProjectUtilities, int>, IProjectUtilitiesRepository
    {
        public ProjectUtilitiesRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<ProjectUtilities>> getByUtilitiesId(int utilitiesId)
        {
            var entities = await DbSet
                .Where(s => s.Status == AppEnum.EntityStatus.NORMAL && s.UtilitiesId == utilitiesId).ToListAsync();

            return entities;
        }

        public async Task<List<int>> getProjectIdByUtilitiesId(int utilitiesId)
        {
            var listProjectId = await DbSet
                .Where(s => s.Status == AppEnum.EntityStatus.NORMAL && s.UtilitiesId == utilitiesId &&
                            s.IsActive == true).Select(s => s.ProjectId).ToListAsync();

            return listProjectId;
        }

        public async Task<ProjectUtilities> getByProjectIdAndUtilitiesId(int projectId, int utilitiesId)
        {
            var entity = await DbSet
                .Where(s => s.Status == AppEnum.EntityStatus.NORMAL && s.UtilitiesId == utilitiesId && s.ProjectId == projectId &&
                            s.IsActive == true).FirstOrDefaultAsync();

            return entity;
        }
    }
}
