using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class UserMappingAsyncRepository : AsyncGenericRepository<UserMapping, int>, IUserMappingAsyncRepository
    {
        public UserMappingAsyncRepository(AppDbContext context) : base(context)
        {

        }
    }
}
