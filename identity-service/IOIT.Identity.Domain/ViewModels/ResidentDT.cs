using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.ViewModels
{
    public class ResidentDT
    {
        public long ResidentId { get; set; }
        public long? ResidentParentId { get; set; }
        public int? RelationshipId { get; set; }
        public string RelationshipName { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public DateTime? DateId { get; set; }
        public string AddressId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avata { get; set; }
        public string Sex { get; set; }
        public string Note { get; set; }
        public DateTime? DateRent { get; set; }
        public int? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }

    }
}
