using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonEmployeeQueue
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? PositionId { get; set; }
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
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
