using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class FriendshipViewModel
    {
        public int id { get; set; }
        public string friendName { get; set; }
        public MessageViewModel lastMessage { get; set; }
        public List<MessageViewModel> messages { get; set; }
    }
}