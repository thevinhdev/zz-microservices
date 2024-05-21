using IOIT.Identity.Domain.Entities;
using System;

namespace IOIT.Identity.Application.Users.ViewModels
{
    public class ResGetUser
    {
        public long UserId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Avata { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? RoleMax { get; set; }
        public int? RoleLevel { get; set; }
        public bool? IsRoleGroup { get; set; }
        public DateTime CreatedAt { get; set; }
        public DepartmentDT department { get; set; }
        public PositionDT position { get; set; }

    }

    public class DepartmentDT
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
    }

    public class PositionDT
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
    }
}
