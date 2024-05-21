using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Users.ViewModels
{
    public class ResUserRegisterApp
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public int? PositionId { get; set; }
        public int? DepartmentId { get; set; }
        public string CardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string KeyRandom { get; set; }
        public byte? TypeThird { get; set; }
        public long? UserMapId { get; set; }
        public byte? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string RegEmail { get; set; }
        public int? RoleMax { get; set; }
        public byte? RoleLevel { get; set; }
        public bool? IsRoleGroup { get; set; }
        public bool? IsPhoneConfirm { get; set; }
        public bool? IsEmailConfirm { get; set; }
        public string RegisterCode { get; set; }
        public int? CountLogin { get; set; }
        public int? LanguageId { get; set; }
        public int? UserCreateId { get; set; }
        public int? UserEditId { get; set; }
        public int? Status { get; set; }
        public int MetaCode { get; set; }
        public string MetaMess { get; set; }
    }
}
