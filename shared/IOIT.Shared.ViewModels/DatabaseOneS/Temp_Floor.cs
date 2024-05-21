using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IOIT.Shared.ViewModels.DatabaseOneS
{
    public class Temp_Floor : Temp_Base
    {
        [Key]
        public int FloorId { get; set; }
        public int? OneSid { get; set; }
        public int? TowerId { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public byte? Status { get; set; }
    }
}
