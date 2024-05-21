using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoUpdateInfoFirebaseQueue
    {
        public long userId { get; set; }
        public int countNotification { get; set; }
        public int countSupportRequire { get; set; }

        public DtoUpdateInfoFirebaseQueue(
            long _userId,
            int _countNotification,
            int _countSupportRequire)
        {
            userId = _userId;
            countNotification = _countNotification;
            countSupportRequire = _countSupportRequire;
        }
    }
}
