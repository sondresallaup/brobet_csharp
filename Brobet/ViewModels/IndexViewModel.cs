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

        public Dictionary<int, List<FixtureViewModel>> fixtures { get; set; }
        public List<Friend> friends { get; set; }

        public DateTime currentDate { get; set; }

        public class Friend
        {
            public int userId { get; set; }
            public string username { get; set; }
            public string dateString { get; set; }
        }

        public string seasonNameFromId(int seasonId)
        {
            if (seasonId == 11983) return "Eliteserien";
            if (seasonId == 12935) return "Ligue 1";
            if (seasonId == 12962) return "Premier League";
            if (seasonId == 13005) return "Bundesliga";
            if (seasonId == 13133) return "La Liga";
            if (seasonId == 13136) return "FA Cup";
            if (seasonId == 13158) return "Serie A";
            if (seasonId == 12950) return "Champions League";
            if (seasonId == 12945) return "Europa League";
            return seasonId.ToString();
        }
    }
}