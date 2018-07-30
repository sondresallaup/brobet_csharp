using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class MessageViewModel
    {
        public int id { get; set; }
        public bool isCurrentUser { get; set; }
        public bool isBet { get; set; }
        public bool accepted { get; set; }
        public string senderName { get; set; }
        public string fixtureName { get; set; }
        public string visitorTeamName { get; set; }
        public string localTeamName { get; set; }
        public string visitorTeamLogo { get; set; }
        public string localTeamLogo { get; set; }
        public DateTime fixtureDate { get; set; }
        public string fixtureDateAsString { get; set; }
        public string messageContent { get; set; }
        public string url { get; set; }
        public string betStatus { get; set; }
        public string fixtureStatus { get; set; }
    }
}