using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IOIT.Shared.ViewModels.DatabaseOneS
{
    public class Temp_Tower : Temp_Base
    {
        [Key]
        public int TowerId { get; set; }
        public int? OneSid { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PersonContact { get; set; }
        public string PhoneContact { get; set; }
        public string Note { get; set; }
        public string LatLong { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public byte? Status { get; set; }
        public string ApiCarParking { get; set; }
        public string UserNameCarParking { get; set; }
        public string PasswordCarParking { get; set; }
    }
}
