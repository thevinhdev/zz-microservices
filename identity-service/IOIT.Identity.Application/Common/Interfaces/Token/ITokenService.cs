using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Common.Interfaces.Token
{
    public interface ITokenService
    {
        ResTokenGeneration GenerateAccessToken(User user, IEnumerable<Claim> extraClaims);
        ResTokenGeneration GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
