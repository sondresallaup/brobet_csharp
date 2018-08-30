using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class BetOverviewViewModel
    {
        public List<Bet> activeBets { get; set; }
        public List<Bet> previousBets { get; set; }
        public List<Bet> sentBetRequests { get; set; }
        public List<Bet> receivedBetRequests { get; set; }
        public User currentUser { get; set; }

        public class Friend
        {
            public int id { get; set; }
            public string username { get; set; }
            public string avatar{ get; set; }
        }

        public class Bet
        {
            public int id { get; set; }
            public Friend friend { get; set; }
            public Fixture fixture { get; set; }
            public bool isFromCurrentUser { get; set; }
            public List<BetObject> currentUserBetObjects { get; set; }
            public User winner { get; set; }

            public class BetObject
            {
                public int type { get; set; }
                public string value { get; set; }
            }
        }

        public class Fixture
        {
            public string name { get; set; }
            public DateTime date { get; set; }
        }
    }
}