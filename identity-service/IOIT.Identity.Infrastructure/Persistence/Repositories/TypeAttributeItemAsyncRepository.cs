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
    public class TypeAttributeItemAsyncRepository : AsyncGenericRepository<TypeAttributeItem, int>, ITypeAttributeItemAsyncRepository
    {
        public TypeAttributeItemAsyncRepository(AppDbContext context) : base(context)
        {

        }

        async Task<TypeAttributeItem> ITypeAttributeItemAsyncRepository.FindByTypeAttributeItemIdAsync(int? typeAttributeItemId, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.TypeAttributeItemId == typeAttributeItemId, cancellationToken);
        }
        public async Task<TypeAttributeItem> GetDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.AsNoTracking().Where(condition).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<List<TypeAttributeItem>> GetListDataByCondition(string condition)
        {
            condition = HttpUtility.UrlDecode(condition);
            var entity = await DbSet.Where(condition).ToListAsync();

            return entity;
        }

    }
}
