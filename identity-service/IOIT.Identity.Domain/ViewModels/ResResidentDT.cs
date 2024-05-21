using IOIT.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.ViewModels
{
    public class ResResidentDT : Resident
    {
        public long ResidentId { get; set; }
        public long? UserId { get; set; }
        public List<ApartmentMapDT> apartments { get; set; }
        public UserDT user { get; set; }
    }

    public class ResResidentIdDT
    {
        public long? ResidentId { get; set; }
        public long? UserId { get; set; }
        public int? ApartmentId { get; set; }
    }
}
