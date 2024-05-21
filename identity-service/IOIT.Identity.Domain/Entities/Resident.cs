using IOIT.Shared.Commons.BaseEntities;
using System;
using static IOIT.Identity.Domain.Enum.DomainEnum;

namespace IOIT.Identity.Domain.Entities
{
    public class Resident : AbstractEntity<long>
    {
        //public long Id { get; set; }
        public int? OneSid { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? CardId { get; set; }
        public DateTime? DateId { get; set; }
        public ResidentRequestIdentifyType? TypeCardId { get; set; }
        public string? AddressId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Avata { get; set; }
        public string? Sex { get; set; }
        public string? Note { get; set; }
        public DateTime? DateRent { get; set; }
        public int? Type { get; set; }
        public int? CountryId { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public int? UserId { get; set; }
        //public byte? Status { get; set; }
    }
}
