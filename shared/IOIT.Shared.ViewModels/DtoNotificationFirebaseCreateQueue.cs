using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels
{
    public class DtoNotificationFirebaseCreateQueue
    {
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public string TargetId { get; set; }
        public string TargetType { get; set; }
        public int? TargetAction { get; set; }
        public int? ProjectId { get; set; }
        public string Logs { get; set; }
        public string Ipaddress { get; set; }
        public int? Time { get; set; }
        public TypeAction? Type { get; set; }
        public long? UserPushId { get; set; }
        public long? UserId { get; set; }

        public DtoNotificationFirebaseCreateQueue(
            string actionName, 
            string actionType, 
            string targetId, 
            string targetType, 
            int? targetAction, 
            int? projectId, 
            string logs, 
            string ipAddress, 
            int time, 
            TypeAction? type, 
            long? userPushId,
            long? userId)
        {
            ActionName = actionName;
            ActionType = actionType;
            TargetId = targetId;
            TargetType = targetType;
            TargetAction = targetAction;
            ProjectId = projectId;
            Logs = logs;
            Ipaddress = ipAddress;
            Time = time;
            Type = type;
            UserPushId = userPushId;
            UserId = userId;
        }
    }
}
