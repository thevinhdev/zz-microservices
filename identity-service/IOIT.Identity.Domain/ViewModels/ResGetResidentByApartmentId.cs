using System;
using IOIT.Identity.Domain.Enum;

namespace IOIT.Identity.Domain.ViewModels
{
    public class ResGetResidentByApartmentId
    {
        public long ResidentId { get; set; }
        public string ResidentName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public DomainEnum.ResidentRequestIdentifyType? IdentifyType { get; set; }
        public string IdentifyCode { get; set; } = string.Empty;
        public int RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public DateTime? IdentifyCreate { get; set; }
        public string IdentifyLoc { get; set; }
    }
}
