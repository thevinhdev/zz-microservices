using IOIT.Identity.Application.Utilities.Queries;

namespace IOIT.Identity.Application.Specifications.PagingSpec
{
    public class UtilitiesFilterWithPagingSpec : BaseSpecification<Domain.Entities.Utilities>
    {
        public UtilitiesFilterWithPagingSpec(GetUtilitiesByPageQuery query)
            : base(c => c.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED)
        {
            if (!string.IsNullOrEmpty(query.query))
            {
                AddQueryString(query.query);
            };

            ApplyOrderBy(s => s.Order);
            ApplyPaging(query.page, query.page_size);
        }
    }
}
