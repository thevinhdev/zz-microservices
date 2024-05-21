using IOIT.Shared.Commons.BaseEntities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IAsyncGenericRepository<TEntity, TId> 
        where TEntity : BaseEntity<TId>
    {
        IQueryable<TEntity> All();

        #region Add
        Task AddAsync(TEntity entity);
        void Add(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        void AddRange(List<TEntity> entities);
        #endregion

        #region Update
        //Task<TEntity> UpdateAsync(TEntity entity);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        Task ExecuteNonQuery(string storeProcedure, params AppSpParameter[] parameters);
        #endregion

        #region Delete
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        #endregion

        Task<int> CountAsync(ISpecification<TEntity> spec);

        #region Get
        Task<TEntity> GetByKeyAsync(TId keyValue);
        Task<IList<TEntity>> GetByKeysAsync(List<TId> keyValues);
        TEntity GetByKey(TId keyValue);
        Task<TEntity> FirstAsync(ISpecification<TEntity> spec);
        Task<TEntity> FindAsync(object[] keyValues);
        Task<IList<TResult>> GetFromSql<TResult>(string storeProcedure, params AppSpParameter[] parameters) where TResult : new();
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<IPagedResult<TEntity>> PaggingAsync(ISpecification<TEntity> spec);
        #endregion
    }
}
