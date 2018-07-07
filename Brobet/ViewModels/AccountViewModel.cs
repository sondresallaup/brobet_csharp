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

        public decimal accountBalance { get; set; }

        public List<Friend> sentFriendRequests { get; set; }
        public List<Friend> receivedFriendRequests { get; set; }
        public List<Friend> friends { get; set; }
        public List<Transaction> transactions { get; set; }

        public class Friend
        {
            public int userId { get; set; }
            public string username { get; set; }
            public string dateString { get; set; }
        }

        public class Transaction
        {
            public int id { get; set; }
            public decimal amount { get; set; }
            public string description { get; set; }
            public string friendName { get; set; }
            public string fixtureTitle { get; set; }
            public string dateString { get; set; }
        }
    }
}