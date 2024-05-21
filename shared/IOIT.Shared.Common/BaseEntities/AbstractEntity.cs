using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.Commons.BaseEntities
{
    public class AbstractEntity<TId> : BaseEntity<TId>
    {
        public virtual long? CreatedById { get; set; }
        public virtual long? UpdatedById { get; set; }
    }

    public class AbstractEntity : BaseEntity<int>
    {
        public virtual long? CreatedById { get; set; }
        public virtual long? UpdatedById { get; set; }
    }
}
