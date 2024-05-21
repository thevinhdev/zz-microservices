using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.Entities
{
    public class ProjectUtilities : AbstractEntity
    {
        public int ProjectId { get; set; }
        public int UtilitiesId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActivedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
