using System;
using System.Collections.Generic;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoUtitlitiesResidentUpdateQueue
    {
        public int ResidentId { get; set; }
        public int? ProjectId { get; set; }
        public long? ResidentParentId { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public DateTime? DateId { get; set; }
        public int? CountryId { get; set; }
        public string AddressId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avata { get; set; }
        public string Sex { get; set; }
        public string Note { get; set; }
        public DateTime? DateRent { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }
        public List<ApartmentMapDT> apartments { get; set; }
        public ResidentIdentifyType? TypeCardId { get; set; }
    }

    public class ApartmentMapDT
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
