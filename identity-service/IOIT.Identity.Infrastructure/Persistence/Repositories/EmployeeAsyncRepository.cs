using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
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
    public class EmployeeAsyncRepository : AsyncGenericRepository<Employee, int>, IEmployeeAsyncRepository
    {
        public EmployeeAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Employee> FindByCodeAsync(string code, int type, CancellationToken cancellationToken)
        {
            if (type == 0)
            {
                return await DbSet.Where(c => c.Code.Trim() == code.Trim() && c.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
            }
            else
            {
                return await DbSet.Where(c => c.Code.Trim() == code.Trim() && c.Id != type && c.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
            }
        }

        public async Task<Employee> FindByPhoneAsync(string phone, int type, CancellationToken cancellationToken)
        {
            if (type == 0)
            {
                return await DbSet.Where(c => c.Phone.Trim() == phone.Trim() && c.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
            }
            else
            {
                return await DbSet.Where(c => c.Phone.Trim() == phone.Trim() && c.Id != type && c.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
            }
        }

        public async Task<IList<Employee>> FindByPositionAsync(int positionId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(c => c.PositionId == positionId).ToListAsync();
        }

        public async Task<Employee> FindByUserMapIdAsync(long? userMapId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(c => c.Id == userMapId).FirstOrDefaultAsync();
        }

        public async Task<IPagedResult<Employee>> GetByPageHandelAsync(FilterPagination paging, int? towerId, int? departmentId, long? userId, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<Employee> pagedResult = new Application.Models.PagedResult<Employee>();
            User user = DbContext.Set<User>().Where(u => u.Id == userId).FirstOrDefault();

            IQueryable<Employee> data = (from e in DbSet
                                         join em in DbContext.Set<EmployeeMap>() on e.Id equals em.EmployeeId
                                         where e.Status != AppEnum.EntityStatus.DELETED
                                         && em.Status != AppEnum.EntityStatus.DELETED
                                         && e.DepartmentId == departmentId
                                         && em.TowerId == towerId
                                         && e.Id != user.UserMapId
                                         select e).AsQueryable();


            if (paging.query != null)
            {
                paging.query = HttpUtility.UrlDecode(paging.query);
            }

            data = data.Where(paging.query);
            pagedResult.RowCount = data.ToList().Count;

            if (paging.page_size > 0)
            {
                if (paging.order_by != null)
                {
                    data = data.OrderBy(paging.order_by).Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
                else
                {
                    data = data.OrderBy("EmployeeId desc").Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
            }
            else
            {
                if (paging.order_by != null)
                {
                    data = data.OrderBy(paging.order_by);
                }
                else
                {
                    data = data.OrderBy("EmployeeId desc");
                }
            }

            if (paging.select != null && paging.select != "")
            {
                paging.select = "new(" + paging.select + ")";
                paging.select = HttpUtility.UrlDecode(paging.select);
                //pagedResult.Results = data.Select(paging.select).ToDynamicArray();
            }
            else
                pagedResult.Results = data.ToList();

            return pagedResult;
        }

    }
}
