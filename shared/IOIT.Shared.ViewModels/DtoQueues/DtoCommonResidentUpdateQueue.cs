using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonResidentUpdateQueue
    {
        public long ResidentId { get; set; }
        public int? OneSid { get; set; }
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
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
