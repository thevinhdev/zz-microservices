using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.BaseEntities;
using IOIT.Shared.ViewModels.DatabaseOneS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class AsyncGenericDataOneSRepository<TEntity> : IAsyncGenericDataOneSRepository<TEntity>
        where TEntity : Temp_Base
    {
        public AppDbContext DbContext
        {
            get;
        }
        public IQueryable<TEntity> DbSet
        {
            get;
        }

        public AsyncGenericDataOneSRepository(AppDbContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }

        public List<TEntity> All()
        {
            return DbSet.ToList();
        }
    }
}
