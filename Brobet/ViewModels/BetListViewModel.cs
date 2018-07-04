using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class BetListViewModel
    {
        public List<BetRequest> bets { get; set; }
        public List<BetRequest> sentBetRequests { get; set; }
        public List<BetRequest> receivedBetRequests { get; set; }

        public class BetRequest
        {
            public int id { get; set; }
            public string fixtureTitle { get; set; }
            public string bet { get; set; }
            public string status { get; set; }
            public string friendName { get; set; }
            public int homeAmount { get; set; }
            public int awayAmount { get; set; }
            public bool? hasWon { get; set; }
        }
    }
}