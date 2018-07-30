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
            scores = f.scores;
            status = f.status;
            startingAt = f.startingAt;
            localTeamName = f.LocalTeam.name;
            localTeamLogo = f.LocalTeam.logoUrl;
            visitorTeamName = f.VisitorTeam.name;
            visitorTeamLogo = f.VisitorTeam.logoUrl;
            time = f.time;
        }

        public int id { get; set; }
        public int localTeamId { get; set; }
        public int visitorTeamId { get; set; }
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
    }
}