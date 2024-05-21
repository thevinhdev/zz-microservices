using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class Department : AbstractEntity
    {
        public int DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }

    }
}
