using IOIT.Shared.Commons.BaseEntities;

namespace IOIT.Identity.Domain.Entities
{
    public class TypeAttributeItem : AbstractEntity
    {
        public int TypeAttributeItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
