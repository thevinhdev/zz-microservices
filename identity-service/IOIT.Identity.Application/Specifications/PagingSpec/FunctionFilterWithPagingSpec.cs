using IOIT.Identity.Application.Functions.Queries;
using IOIT.Identity.Application.Functions.ViewModels;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Specifications.PagingSpec
{
    public class FunctionFilterWithPagingSpec : BaseSpecification<Function>
    {
        public FunctionFilterWithPagingSpec(GetFunctionByPagingQuery query)
            : base(c => c.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED)
        {
            if (!string.IsNullOrEmpty(query.query))
            {
                AddQueryString(query.query);
            };
            if (!string.IsNullOrEmpty(query.order_by))
            {
                AddOrderbY(query.order_by);
            }
            else
            {
                // ApplyOrderBy(s => s.Id);
            }
            ApplyPaging(query.page, query.page_size);
        }
    }
}
