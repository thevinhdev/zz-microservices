using IOIT.Identity.Domain.Entities.Indentity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Identity.Managers
{
    public class IdentityAppAccountConfirmation : IUserConfirmation<User>
    {
        public Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(user.IsActive);
        }
    }
}
