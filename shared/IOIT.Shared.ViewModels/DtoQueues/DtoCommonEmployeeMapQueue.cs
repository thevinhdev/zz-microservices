using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonEmployeeMapQueue
    {
        public int EmployeeMapId { get; set; }
        public int? EmployeeId { get; set; }
        public int? TowerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
