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
        public string messageContent { get; set; }
        public string url { get; set; }
    }
}