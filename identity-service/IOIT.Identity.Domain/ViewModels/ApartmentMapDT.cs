using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.ViewModels
{
    public class ApartmentMapDT
    {
        public Guid ApartmentMapId { get; set; }
        public int? ApartmentId { get; set; }
        public int? FloorId { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; } = string.Empty;
        public byte? Type { get; set; }
        public int? RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public DateTime? DateRent { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
