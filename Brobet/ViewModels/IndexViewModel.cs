using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.ViewModels
{
    public class IndexViewModel
    {
        public int daysFromNow { get; set; }

        public Dictionary<int, FixtureViewModel> fixtures { get; set; }
        public List<Friend> friends { get; set; }

        public class Friend
        {
            public int userId { get; set; }
            public string username { get; set; }
            public string dateString { get; set; }
        }
    }
}