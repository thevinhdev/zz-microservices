using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class AsyncGuidRepository<TEntity> : AsyncGenericRepository<TEntity, Guid>, IAsyncGuidRepository<TEntity>
        where TEntity : BaseEntity<Guid>
    {
        public AsyncGuidRepository(AppDbContext context) : base(context)
        {

        }
    }
}
