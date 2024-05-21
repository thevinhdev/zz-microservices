using System.ComponentModel.DataAnnotations;

namespace IOIT.Identity.Api.Models
{
    public class ReqRegister
    {
        [Required]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}
