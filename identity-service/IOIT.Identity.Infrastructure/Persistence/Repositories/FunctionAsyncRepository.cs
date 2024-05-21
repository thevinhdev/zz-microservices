using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class FunctionAsyncRepository : AsyncGenericRepository<Function, int>, IFunctionAsyncRepository
    {
        public FunctionAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Function> FindByCodeAsync(string code, int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code && c.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
            }
            else
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code && c.Id != id && c.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
            }
        }

        public async Task<IList<Function>> GetListAllFunctionAsync(CancellationToken cancellationToken)
        {
            return await DbSet.Where(c =>  c.Status == AppEnum.EntityStatus.NORMAL).ToListAsync();
        }

        public async Task<List<Function>> GetListFunctionByParentIdAsync(int functionId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(c =>c.FunctionParentId == functionId && c.Status == AppEnum.EntityStatus.NORMAL).ToListAsync();
        }

        public async Task<IList<ResSmallFunction>> GetListFunctionAsync(List<ResSmallFunction> dt, int functionId, int level, int roleMax, CancellationToken cancellationToken)
        {
            var index = level + 1;
            try
            {
                IEnumerable<Function> data;
                if (roleMax == 1)
                {
                    data = await DbSet.Where(e => e.FunctionParentId == functionId && e.Status != AppEnum.EntityStatus.DELETED).ToListAsync();
                }
                else
                {
                    data = (from fr in DbContext.Set<FunctionRole>()
                            join f in DbSet on fr.FunctionId equals f.Id
                            where fr.TargetId == roleMax && fr.Type == (int)AppEnum.TypeFunction.FUNCTION_ROLE
                            && fr.Status != AppEnum.EntityStatus.DELETED && fr.Status != AppEnum.EntityStatus.DELETED
                            && f.FunctionParentId == functionId
                            select f).ToList();
                }

                if (data != null)
                {
                    if (data.Count() > 0)
                    {
                        foreach (var item in data)
                        {
                            ResSmallFunction function = new ResSmallFunction();
                            function.FunctionId = item.Id;
                            function.Code = item.Code;
                            function.Name = item.Name;
                            function.Level = level;
                            function.IsParamRoute = item.IsParamRoute;
                            //function.children = listFunction(item.FunctionId);
                            dt.Add(function);
                            try
                            {
                                await GetListFunctionAsync(dt, item.Id, index, roleMax, cancellationToken);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }

            return dt;
        }

    }
}
