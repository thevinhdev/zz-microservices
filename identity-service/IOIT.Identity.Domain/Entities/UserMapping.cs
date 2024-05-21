using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.Entities
{
    public class UserMapping : AbstractEntity
    {
        //public int Id { get; set; }
        public long? UserId { get; set; }
        public int? TargetId { get; set; }
        public byte? TargetType { get; set; }
        //public int? UserIdCreatedId { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public byte? Status { get; set; }
    }
}
