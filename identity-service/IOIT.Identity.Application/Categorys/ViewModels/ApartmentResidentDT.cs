using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Models
{
    public class ApartmentResidentDT
    {
        public Guid ApartmentMapId { get; set; }
        public int? ApartmentId { get; set; }
        public int? FloorId { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public byte? Type { get; set; }
        public int? RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public DateTime? DateRent { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
