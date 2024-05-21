using IOIT.Identity.Domain.ViewModels;
using System;
using System.Collections.Generic;
using static IOIT.Identity.Domain.Enum.DomainEnum;

namespace IOIT.Identity.Application.Residents.ViewModels
{
    public class ReqRequestCreateNewResident
    {
        public int? ProjectId { get; set; }
        public long? ResidentParentId { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public DateTime? DateId { get; set; }
        public ResidentRequestIdentifyType? TypeCardId { get; set; }
        public string AddressId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avata { get; set; }
        public string Sex { get; set; }
        public string Note { get; set; }
        public DateTime? DateRent { get; set; }
        public int? CountryId { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }
        public List<ApartmentMapDT> apartments { get; set; }
    }
}
