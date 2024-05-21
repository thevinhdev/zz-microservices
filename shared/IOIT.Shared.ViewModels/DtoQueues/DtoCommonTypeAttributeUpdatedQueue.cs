using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonTypeAttributeUpdatedQueue
    {
        public int TypeAttributeId { get; set; }
        public string Name { get; set; }
        public bool? IsUpdate { get; set; }
        public bool? IsDelete { get; set; }
        public int? TypeAttribuiteParentId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
