using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IOIT.Shared.ViewModels.DatabaseOneS
{
    public class Temp_ApartmentMap : Temp_Base
    {
        [Key]
        public Guid ApartmentMapId { get; set; }
        public int? ApartmentId { get; set; }
        public int? FloorId { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public long? ResidentId { get; set; }
        public int? RelationshipId { get; set; }
        public DateTime? DateRent { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public byte? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public byte? Status { get; set; }
    }
}
