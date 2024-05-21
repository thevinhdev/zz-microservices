using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class Role : AbstractEntity
    {
        //public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte? LevelRole { get; set; }
        public string Note { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public int? UserEditId { get; set; }
        //public int? UserId { get; set; }
        //public byte? Status { get; set; }
    }
}
