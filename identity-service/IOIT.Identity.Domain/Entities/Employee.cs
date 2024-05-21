using IOIT.Shared.Commons.BaseEntities;
using System;

namespace IOIT.Identity.Domain.Entities
{
    public class Employee : AbstractEntity
    {
        //public int Id { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? PositionId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public byte? TypeEmployee { get; set; }
        public bool? IsMain { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public int? UserId { get; set; }
        //public byte? Status { get; set; }
    }
}
