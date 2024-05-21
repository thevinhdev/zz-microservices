using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoIdentityApartmentMapQueue
    {
        public Guid Id { get; set; }
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
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
    }

    public class DtoCommonApartmentMapQueue
    {
        public Guid Id { get; set; }
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
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
