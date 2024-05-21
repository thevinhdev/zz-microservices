using IOIT.Shared.Commons.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.Entities
{
    public class Apartment : AbstractEntity
    {
        public int ApartmentId { get; set; }
        public int? FloorId { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
