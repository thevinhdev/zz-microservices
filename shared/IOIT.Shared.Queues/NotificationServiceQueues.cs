using IOIT.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.Queues
{
    public class NotificationServiceQueues
    {
        public class NotificationActionCreateQueue
        {
            public static string Name = "notification.action.create.queue";
            public DtoNotificationActionCreateQueue Payload { get; set; }

        }

        public class NotificationFirebaseCreateQueue
        {
            public static string Name = "notification.firebase.create.queue";
            public DtoNotificationActionCreateQueue Payload { get; set; }

        }
    }
}
