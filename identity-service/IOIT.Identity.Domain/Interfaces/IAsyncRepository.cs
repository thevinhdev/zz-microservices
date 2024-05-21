using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IAsyncRepository<TEntity> : IAsyncGenericRepository<TEntity, int>
       where TEntity : BaseEntity<int>
    {
    }
}
