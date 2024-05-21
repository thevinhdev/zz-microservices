using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoUtilitiesSlaEndQueue
    {
        public SlaServiceId Id { get; set; }
        public SlaServiceDetailStep Step { get; set; }
        public long RefId { get; set; }
        public string RefData { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Note { get; set; }
    }
}
