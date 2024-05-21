using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonTypeAttributeItemUpdatedQueue
    {
        public int TypeAttributeItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TypeAttributeId { get; set; }
        public string Icon { get; set; }
        public int? Location { get; set; }
        public bool? IsActive { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
