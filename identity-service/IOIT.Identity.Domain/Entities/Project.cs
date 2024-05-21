using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class Project : AbstractEntity
    {
        public int ProjectId { get; set; }
        public int? OneSId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
