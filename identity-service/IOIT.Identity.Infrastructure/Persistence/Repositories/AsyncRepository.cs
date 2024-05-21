using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.BaseEntities;
using System;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class AsyncRepository<TEntity> : AsyncGenericRepository<TEntity, int>, IAsyncRepository<TEntity>
         where TEntity : BaseEntity<int>
    {
        public AsyncRepository(AppDbContext context) : base(context)
        {

        }
    }
}
