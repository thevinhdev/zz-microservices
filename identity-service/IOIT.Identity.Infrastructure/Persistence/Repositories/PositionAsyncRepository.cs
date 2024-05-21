using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class PositionAsyncRepository : AsyncGenericRepository<Position, int>, IPositionAsyncRepository
    {
        public PositionAsyncRepository(AppDbContext context) : base(context)
        {

        }

    }
}
