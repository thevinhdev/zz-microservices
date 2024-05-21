using IOIT.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.ViewModels
{
    public class UserDT : User
    {
        public long UserId { get; set; }
        public int? UserCreateId { get; set; }
        public int? UserEditId { get; set; }
        public int? Status { get; set; }
    }
}
