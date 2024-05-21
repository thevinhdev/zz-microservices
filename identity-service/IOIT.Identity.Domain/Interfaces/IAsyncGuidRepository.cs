using IOIT.Shared.Commons.BaseEntities;
using System;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IAsyncGuidRepository<TEntity> : IAsyncGenericRepository<TEntity, Guid>
        where TEntity : BaseEntity<Guid>
    {
    }
}
