using System;

namespace IOIT.Identity.Application.Users.ViewModels
{
    public class ResCreateUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LanguageIsoCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
