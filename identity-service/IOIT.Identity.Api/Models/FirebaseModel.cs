using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Models
{
    public class FirebaseModel
    {
    }

    [FirestoreData]
    public partial class ConversationFS
    {
        [FirestoreProperty]
        public string conversationId { get; set; }
        [FirestoreProperty]
        public int createdAt { get; set; }
        [FirestoreProperty]
        public string contents { get; set; }
        [FirestoreProperty]
        public int type { get; set; }
        [FirestoreProperty]
        public UserFS user { get; set; }
    }

    [FirestoreData]
    public partial class UserFS
    {
        [FirestoreProperty]
        public long userId { get; set; }
        [FirestoreProperty]
        public string fullname { get; set; }
        [FirestoreProperty]
        public string avata { get; set; }
        [FirestoreProperty]
        public int countNotification { get; set; }
        [FirestoreProperty]
        public int countSupportRequire { get; set; }
        [FirestoreProperty]
        public int status { get; set; }
    }
}
