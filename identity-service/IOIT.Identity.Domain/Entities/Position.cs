using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class Position : AbstractEntity
    { 
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ProjectId { get; set; }
        public int? TowerId { get; set; }
        public int? LevelId { get; set; }
        public string Note { get; set; }

    }
}
