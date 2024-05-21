using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IAsyncLongRepository<TEntity> : IAsyncGenericRepository<TEntity, long>
        where TEntity : BaseEntity<long>
    {
    }
}
