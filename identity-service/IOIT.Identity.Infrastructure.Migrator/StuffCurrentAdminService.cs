using IOIT.Identity.Application.Common.Interfaces;
using System;
using System.Security.Claims;

namespace IOIT.Identity.Infrastructure.Migrator
{
    public class StuffCurrentAdminService<TId> : ICurrentAdminService<TId>
    {
        public ClaimsPrincipal Principal => throw new NotImplementedException();
        public bool HasPrincipal => throw new NotImplementedException();
        public TId AdminId => default(TId);
        public string UserName => null;
        public string Email => throw new NotImplementedException();
        public string Phone => throw new NotImplementedException();
        public string FullName => throw new NotImplementedException();
        public string UserId => throw new NotImplementedException();
    }
}
