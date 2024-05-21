using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.Entities
{
    public class FunctionRole : AbstractEntity
    {
        //public int Id { get; set; }
        public long TargetId { get; set; }
        public int FunctionId { get; set; }
        public string ActiveKey { get; set; }
        public byte? Type { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public int? UserId { get; set; }
        //public byte? Status { get; set; }

        public virtual Function Function { get; set; }
    }
}
