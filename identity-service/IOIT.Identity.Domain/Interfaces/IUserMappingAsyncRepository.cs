using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IUserMappingAsyncRepository : IAsyncGenericRepository<UserMapping, int>
    {
    }
}
