using IOIT.Identity.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IAccountAsyncRepository
    {
        Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken);
        Task<User> CreateAsync(User User, CancellationToken cancellationToken);
        Task<User> FindByUserCode(string userCode, CancellationToken cancellationToken);
        User HashPassword(User User, string password, CancellationToken cancellationToken);
        bool VerifyPassword(User User, string password, CancellationToken cancellationToken);
        Task<User> FindByReferralCodeAsync(string userName, CancellationToken cancellationToken);

    }
}
