using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class EmployeeMapAsyncRepository : AsyncGenericRepository<EmployeeMap, int>, IEmployeeMapAsyncRepository
    {
        public EmployeeMapAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<EmployeeMap>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(ap => ap.EmployeeId == employeeId && ap.Status != AppEnum.EntityStatus.DELETED).ToListAsync();
        }

        public async Task<EmployeeMap> GetEmployeeTowerAsync(int employeeId, int towerId, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
