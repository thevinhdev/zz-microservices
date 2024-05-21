using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.ViewModels
{
    public class DepartmentMap
    {
        public int? DepartmentId { get; set; }
        public int? RequireTypeId { get; set; }
    }

    public class EmployeeMap
    {
        public int? EmployeeId { get; set; }
        public int? TowerId { get; set; }
    }

    public class UserReceiveNotification
    {
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Avata { get; set; }
    }

    public class EmployeeRequireSp
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Avata { get; set; }
        public int? DepartmentId { get; set; }
        public byte? TypeEmployee { get; set; }
    }
}
