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
    public class DepartmentAsyncRepository : AsyncGenericRepository<Department, int>, IDepartmentAsyncRepository
    {
        public DepartmentAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Department> FindByDepartmentIdAsync(int? departmentId, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.DepartmentId == departmentId, cancellationToken);
        }

        public async Task<Department> GetDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.AsNoTracking().Where(condition).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<List<Department>> GetListDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.Where(condition).ToListAsync();

            return entity;
        }
    }
}
