using IOIT.Identity.Application.Models;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence
{
    public class SpecificationEvaluator<TEntity, TId>
        where TEntity : BaseEntity<TId>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = GetQueryEx(inputQuery, specification);

            if (specification.IsPagingEnabled)
            {
                var skip = (specification.CurrentPage - 1) * specification.PageSize;
                query = query.Skip(skip)
                            .Take(specification.PageSize);
            }

            return query;
        }

        public static async Task<IPagedResult<TEntity>> PaggingAsync(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = GetQueryEx(inputQuery, specification);
            var result = new Application.Models.PagedResult<TEntity>();
            result.CurrentPage = specification.CurrentPage;
            result.PageSize = specification.PageSize == 0 ? 20 : specification.PageSize;
            result.RowCount = query.Count();
            var pageCount = (double)result.RowCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (specification.CurrentPage - 1) * result.PageSize;

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(skip)
                            .Take(specification.PageSize);
            }

            result.Results = await query.ToListAsync();

            return result;
        }

        private static IQueryable<TEntity> GetQueryEx(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));


            if (!string.IsNullOrEmpty(specification.QueryString))
            {
                query = query.Where(specification.QueryString);
            }

            if (!string.IsNullOrEmpty(specification.OrderbyString))
            {
                query = query.OrderBy(specification.OrderbyString);
            }

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            return query;
        }
    }
}
