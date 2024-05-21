using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.Employees.ViewModels
{
    public class ResEmployeeMapDT
    {
        public int EmployeeMapId { get; set; }
        public int? EmployeeId { get; set; }
        public int? TowerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }
    }
}
