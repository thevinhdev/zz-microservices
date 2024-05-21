using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonDepartmentMapUpdatedQueue
    {
        public Guid DepartmentMapId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RequireTypeId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
