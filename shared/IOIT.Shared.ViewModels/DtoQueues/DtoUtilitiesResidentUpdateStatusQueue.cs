using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoUtilitiesResidentUpdateStatusQueue
    {
        public long ResidentId { get; set; }
        public EntityStatus Status { get; set; }
        public long UserId { get; set; }
    }
}
