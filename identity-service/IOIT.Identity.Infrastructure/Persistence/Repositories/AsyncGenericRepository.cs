using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.BaseEntities;
using IOIT.Shared.Commons.Enum;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class AsyncGenericRepository<TEntity, TId> : IAsyncGenericRepository<TEntity, TId>
        where TEntity : BaseEntity <TId>
    {
        public DbContext DbContext
        {
            get;
        }
        public IQueryable<TEntity> DbSet
        {
            get;
        }

        public AsyncGenericRepository(DbContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
        {
            return DbSet;
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public void AddRange(List<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await DbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(List<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity> GetByKeyAsync(TId keyValue)
        {
            return await DbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id.Equals(keyValue) && x.Status != EntityStatus.DELETED);
        }

        public TEntity GetByKey(TId keyValue)
        {
            return DbSet.SingleOrDefault(x => x.Id.Equals(keyValue));
        }
        public async Task<TEntity> FirstAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<TEntity> FindAsync(object[] keyValues)
        {
            return await DbContext.Set<TEntity>().FindAsync(keyValues);
        }
        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<IPagedResult<TEntity>> PaggingAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecificationPaggingAsync(spec);
        }
        public void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        //public Task<TEntity> UpdateAsync(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        public void UpdateRange(List<TEntity> entities)
        {
            DbContext.UpdateRange(entities);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity, TId>.GetQuery(DbSet.AsQueryable(), spec);
        }
        private async Task<IPagedResult<TEntity>> ApplySpecificationPaggingAsync(ISpecification<TEntity> spec)
        {
            return await SpecificationEvaluator<TEntity, TId>.PaggingAsync(DbSet.AsQueryable(), spec);
        }

        public async Task<IList<TResult>> GetFromSql<TResult>(string storeProcedure, params AppSpParameter[] parameters) where TResult : new()
        {
            return await DbContext.GetFromSqlAsync<TResult>(storeProcedure, parameters);
        }

        public async Task<IList<TEntity>> GetByKeysAsync(List<TId> keyValues)
        {
            return await DbSet.Where(c => keyValues.Contains(c.Id)).ToListAsync();
        }

        public async Task ExecuteNonQuery(string storeProcedure, params AppSpParameter[] parameters)
        {
            await DbContext.ExecuteNonQuery(storeProcedure, parameters);
        }
    }
}
