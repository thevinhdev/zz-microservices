using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using Microsoft.EntityFrameworkCore;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class UtilitiesRepository : AsyncGenericRepository<Domain.Entities.Utilities, int>, IUtilitiesRepository
    {
        public UtilitiesRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Domain.Entities.Utilities> getUtilitiesByNameOrCode(string name, string code, CancellationToken cancellationToken)
        {
            var entity = await DbSet
                .Where(s => (s.Name.Equals(name) || s.Code.Equals(code)) && s.Status == AppEnum.EntityStatus.NORMAL)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<Domain.Entities.Utilities> checkExistUtilitiesByNameOrCode(string name, string code, int id, CancellationToken cancellationToken)
        {
            var entity = await DbSet
                .Where(s => (s.Name.Equals(name) || s.Code.Equals(code)) 
                            && s.Status == AppEnum.EntityStatus.NORMAL
                            && s.Id != id)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<List<Domain.Entities.Utilities>> getUtilitiesByProjectId(int projectId, CancellationToken cancellationToken)
        {
            var entities = await DbSet
                .Join(DbContext.Set<ProjectUtilities>(),
                    u => u.Id,
                    pu => pu.UtilitiesId,
                    (u, pu) => new
                    {
                        u, pu
                    })
                .Where(s => s.pu.Status == AppEnum.EntityStatus.NORMAL &&
                            s.pu.IsActive == true &&
                            s.u.Status == AppEnum.EntityStatus.NORMAL &&
                            s.pu.ProjectId == projectId)
                .Select(s => s.u)
                //.Distinct()
                .OrderBy(s => s.Type)
                .ThenBy(x => x.Order)
                .ToListAsync();

            return entities;
        }
    }
}
