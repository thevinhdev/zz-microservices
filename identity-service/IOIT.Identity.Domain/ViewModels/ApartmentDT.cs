using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Domain.ViewModels
{
    public class ApartmentDT
    {
        public int? ApartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? FloorId { get; set; }
        public string FloorName { get; set; }
        public int? TowerId { get; set; }
        public string TowerName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public int? Status { get; set; }
        public byte? TypeUser { get; set; }
    }
}
