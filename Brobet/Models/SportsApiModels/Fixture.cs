using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Models.SportsApiModels
{
    public class Fixture : BaseSportsApiModel
    {
        public int id { get; set; }
        public int? season_id { get; set; }
        public int? stage_id { get; set; }
        public int? round_id { get; set; }
        public int? group_id { get; set; }
        public int? venue_id { get; set; }
        public int? referee_id { get; set; }
        public int localteam_id { get; set; }
        public int visitorteam_id { get; set; }

        public Time time { get; set; }
        public Scores scores { get; set; }
        public Standings standings { get; set; }
        public DataObject<Event> events{ get; set; }

        public class Time
        {
            public string status { get; set; }
            public StartingAt starting_at { get; set; }
            public int? minute { get; set; }
            public int? second { get; set; }
            public int? added_time { get; set; }
            public int? extra_minute { get; set; }
            public int? injury_time { get; set; }

            public class StartingAt
            {
                public string date_time { get; set; }
                public string date { get; set; }
                public string time { get; set; }
                public int? timestamp { get; set; }
                public string timezone { get; set; }
            }
        }

        public class Scores
        {
            public int? localteam_score { get; set; }
            public int? visitorteam_score { get; set; }
            public int? localteam_pen_score { get; set; }
            public int? visitorteam_pen_score { get; set; }
            public string ht_score { get; set; }
            public string ft_score { get; set; }
            public string et_score { get; set; }
        }

        public class Standings
        {
            public int? localteam_position { get; set; }
            public int? visitorteam_position { get; set; }
        }

        public class Event : BaseSportsApiModel
        {
            public Int64 id { get; set; }
            public int team_id { get; set; }
            public string type { get; set; }
            public int? player_id { get; set; }
            public string player_name { get; set; }
            public int? related_player_id { get; set; }
            public string related_player_name { get; set; }
            public int? minute { get; set; }
            public int? extra_minute { get; set; }
            public string header { get; set; }
            public bool? injuried { get; set; }
            public string result { get; set; }
        }
    }
}