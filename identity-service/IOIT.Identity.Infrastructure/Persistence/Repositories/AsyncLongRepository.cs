using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class AsyncLongRepository<TEntity> : AsyncGenericRepository<TEntity, long>, IAsyncLongRepository<TEntity>
        where TEntity : BaseEntity<long>
    {
        public AsyncLongRepository(AppDbContext context) : base(context)
        {

        }
    }
}
