using IOIT.Identity.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IRoleAsyncRepository : IAsyncGenericRepository<Role, int>
    {
        Task<Role> FindByCodeAsync(string code, int roleId, CancellationToken cancellationToken);
    }
}
