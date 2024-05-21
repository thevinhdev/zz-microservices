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
    public class FloorAsyncRepository : AsyncGenericRepository<Floor, int>, IFloorAsyncRepository
    {
        public FloorAsyncRepository(AppDbContext context) : base(context)
        {

        }

        async Task<Floor> IFloorAsyncRepository.FindByFloorIdAsync(int floorId, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.FloorId == floorId, cancellationToken);
        }

        public async Task<Floor> GetDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.AsNoTracking().Where(condition).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<List<Floor>> GetListDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.Where(condition).ToListAsync();

            return entity;
        }

    }
}
