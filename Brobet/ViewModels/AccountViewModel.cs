using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class AccountViewModel
    {
        public bool isMe { get; set; }
        public int userId { get; set; }
        public string username { get; set; }

        public List<Friend> sentFriendRequests { get; set; }
        public List<Friend> receivedFriendRequests { get; set; }
        public List<Friend> friends { get; set; }

        public class Friend
        {
            public int userId { get; set; }
            public string username { get; set; }
            public string dateString { get; set; }
        }
    }
}