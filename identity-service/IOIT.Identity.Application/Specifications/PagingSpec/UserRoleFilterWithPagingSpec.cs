﻿using IOIT.Identity.Application.UserRoles.Queries;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Specifications.PagingSpec
{
    public class UserRoleFilterWithPagingSpec : BaseSpecification<User>
    {
        public UserRoleFilterWithPagingSpec(GetUserRoleByPagingQuery query)
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
