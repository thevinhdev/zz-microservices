using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using IOIT.Identity.Application.Common.Extensions;
using System.Security.Claims;

namespace IOIT.Identity.Api.Services
{
    public class CurrentUserService<TId> : ICurrentAdminService<TId>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            Principal = _httpContextAccessor.HttpContext?.User;
        }
        public ClaimsPrincipal Principal { get; }

        public string Email => FindFirstValue(ClaimTypes.Email);

       // public string FirstName => FindFirstValue("firstname");

        public string FullName => FindFirstValue("fullname");

       // public string LastName => FindFirstValue(ClaimTypes.Surname);

        //public string MiddleName => FindFirstValue("middlename");

        public string Phone => FindFirstValue(ClaimTypes.MobilePhone);

        public string UserId  => FindFirstValue("UserId");

        public TId AdminId
        {
            get
            {

                var id = FindFirstValue("id");
                return (string.IsNullOrEmpty(id) ? default : id.ConvertTo<TId>());
            }
        }
        public string UserName => FindFirstValue(ClaimTypes.Name);

        public bool HasPrincipal => Principal != null;

        public string LanguageIsoCode => _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Accept-Language")
                                            ? _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString() : "en";//Default is Engish

        public bool IsInRole(string roleName)
        {
            return Principal != null && Principal.IsInRole(roleName);
        }

        private string FindFirstValue(string claimType)
        {
            if (Principal == null)
                throw new ValidationException(claimType, "Principal has not been initialized.");

            if (!Principal.Identity.IsAuthenticated) return string.Empty;

            var claim = Principal.FindFirst(claimType);
            if (claim == null)
                throw new ValidationException(claimType, $"Claim '{claimType}' was not found.");

            return claim.Value;
        }
    }
}
