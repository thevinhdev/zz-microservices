using IOIT.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IPositionAsyncRepository : IAsyncGenericRepository<Position, int>
    {
    }
}
