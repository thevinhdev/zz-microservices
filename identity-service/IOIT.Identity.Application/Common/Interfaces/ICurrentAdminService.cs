using System.Security.Claims;

namespace IOIT.Identity.Application.Common.Interfaces
{
    public interface ICurrentAdminService<TId>
    {
        ClaimsPrincipal Principal { get; }
        bool HasPrincipal { get; }
        TId AdminId { get; }
        string UserId { get; }
        string UserName { get; }
        string Email { get; }
        string Phone { get; }
        string FullName { get; }
    }
}
