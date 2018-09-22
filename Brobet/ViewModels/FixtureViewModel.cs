using Brobet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Brobet.ViewModels
{
    public class FixtureViewModel
    {
        public FixtureViewModel()
        {

        }

        public FixtureViewModel(Fixture f)
        {
            id = f.id;
            seasonId = f.seasonId;
            scores = f.scores;
            status = f.status;
            startingAt = f.startingAt;
            localTeamName = f.LocalTeam.name;
            localTeamLogo = f.LocalTeam.logoUrl;
            visitorTeamName = f.VisitorTeam.name;
            visitorTeamLogo = f.VisitorTeam.logoUrl;
            time = f.time;
            homeOdds = f.homeOdds;
            drawOdds = f.drawOdds;
            awayOdds = f.awayOdds;
            deleted = f.deleted.HasValue && f.deleted.Value;
        }

        public int id { get; set; }
        public int localTeamId { get; set; }
        public int visitorTeamId { get; set; }
        public int? seasonId { get; set; }
        public string scores { get; set; }
        public string time { get; set; }
        public string standings { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? startingAt { get; set; }
        public string startingAtAsString { get; set; }
        public string status { get; set; }
        public DateTime? date { get; set; }

        public string localTeamName { get; set; }
        public string localTeamLogo { get; set; }

        public string visitorTeamName { get; set; }
        public string visitorTeamLogo { get; set; }

        public double? homeOdds { get; set; }
        public double? drawOdds { get; set; }
        public double? awayOdds { get; set; }
        public bool deleted { get; set; }

        public int minute
        {
            get
            {
                var minute = Json.Decode(this.time)["minute"];
                return minute != null ? minute : 0;
            }
        }

        public int localTeamScore
        {
            get
            {
                return Json.Decode(this.scores)["localteam_score"];
            }
        }

        public int visitorTeamScore
        {
            get
            {
                return Json.Decode(this.scores)["visitorteam_score"];
            }
        }

        public string staringAtString
        {
            get
            {
                return this.startingAt.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            }
        }

        public bool hasProbability
        {
            get
            {
                return this.homeOdds.HasValue;
            }
        }

        public int homeProbability
        {
            get
            {
                if (!hasProbability) return 0;
                return (int)Math.Round(((1.0 / this.homeOdds.Value) / this.totalProbability) *100.0, 0);
            }

        }

        public int drawProbability
        {
            get
            {
                if (!hasProbability) return 0;
                return (int)Math.Round(((1.0 / this.drawOdds.Value) / this.totalProbability) *100.0, 0);
            }

        }

        public int awayProbability
        {
            get
            {
                if (!hasProbability) return 0;
                return (int)Math.Round(((1.0 / this.awayOdds.Value) / this.totalProbability) * 100.0, 0);
            }

        }

        private double totalProbability
        {
            get
            {
                return (1.0 / this.homeOdds.Value) + (1.0 / this.drawOdds.Value) + (1.0 / this.awayOdds.Value);
            }
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