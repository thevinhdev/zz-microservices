using IOIT.Identity.Application.Projects.Queries;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Specifications.PagingSpec
{
    public class ProjectFilterWithPagingSpec : BaseSpecification<Project>
    {
        public ProjectFilterWithPagingSpec(GetProjectByPagingQuery query)
            : base(c => c.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED)
        {
            if (!string.IsNullOrEmpty(query.query))
            {
                AddQueryString(query.query);
            };

            ApplyPaging(query.page, query.page_size);
        }
    }
}
