using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Employees.ViewModels
{
    public class ResEmployee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public byte? TypeEmployee { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public int? Status { get; set; }
        public List<ResEmployeeMap> employeeMaps { get; set; }
    }

    public class ResEmployeeMap
    {
        public int? TowerId { get; set; }
        public string Name { get; set; }
        public bool? Selected { get; set; }
    }

    public class ResEmployeeLists
    {
        public List<ResEmployee> Results { get; set; }
        public int RowCount { get; set; }

    }

    public class ResEmpHandle
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
    }

    public class ResEmpProject
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
    }

}
