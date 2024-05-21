using IOIT.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IOIT.Identity.Application.Specifications
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }
        public Expression<Func<TEntity, object>> GroupBy { get; private set; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; } = 1;
        public bool IsPagingEnabled { get; private set; } = false;
        public string QueryString { get; private set; }
        public string OrderbyString { get; private set; }
        public string Select { get; private set; }

        protected BaseSpecification(Expression<Func<TEntity, bool>> cirteria)
        {
            Criteria = cirteria;
        }

        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
        protected virtual void AddQueryString(string queryString)
        {
            QueryString = queryString;
        }

        protected virtual void AddOrderbY(string orderbyString)
        {
            OrderbyString = orderbyString;
        }
        protected virtual void AddSelect(string select)
        {
            Select = select;
        }

        protected virtual void ApplyPaging(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            IsPagingEnabled = true;
        }
        protected virtual void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected virtual void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        protected virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }
    }
}
