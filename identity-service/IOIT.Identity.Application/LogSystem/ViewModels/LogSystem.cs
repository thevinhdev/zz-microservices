using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Identity.Application.LogSystem.ViewModels
{
    public class LogSystemRes
    {
        public string NotificationId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TargetId { get; set; }
        public int? Type { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? UserPushId { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string ActionType { get; set; }
        public string Url { get; set; }
        public dynamic user { get; set; }
        public dynamic TypeNotification { get; set; }
    }

    public class ResTotal {
        public List<LogSystemRes> data { get; set; }
        public int total { get; set; }
    }
}
