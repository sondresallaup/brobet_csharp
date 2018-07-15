using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class BetRequestViewModel
    {
        public int betId { get; set; }
        public int betRequestId { get; set; }

        public FixtureViewModel fixture { get; set; }
        public List<Friend> friends { get; set; }
        public bool isFromUser { get; set; }
        public bool initial { get; set; }
        public bool editable { get; set; }
        public bool accepted { get; set; }

        public int fromAmount { get; set; }
        public int toAmount { get; set; }

        public string[] fromBets { get; set; }
        public string[] toBets { get; set; }

        public string backUrl { get; set; }

        public Friend friend { get; set; }

        public class Friend
        {
            public int userId { get; set; }
            public string username { get; set; }
            public string dateString { get; set; }
        }
    }
}