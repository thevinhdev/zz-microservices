using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class Tower : AbstractEntity
    {
        public int TowerId { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
