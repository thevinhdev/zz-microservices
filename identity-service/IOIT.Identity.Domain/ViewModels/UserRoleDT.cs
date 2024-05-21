using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.ViewModels
{
    public class UserRoleDT
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public DateTime? Birthday { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
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
        public long? UserCreateId { get; set; }
        public long? UserEditId { get; set; }
        public int? Status { get; set; }
        public List<RoleDT> listRole { get; set; }
        public List<FunctionRoleDT> listFunction { get; set; }
        //public List<ListProject> listProject { get; set; }
        //public List<ListTower> listTower { get; set; }
    }

    public partial class FunctionRoleDT
    {
        public int FunctionRoleId { get; set; }
        public int TargetId { get; set; }
        public int FunctionId { get; set; }
        public string ActiveKey { get; set; }
    }

}
