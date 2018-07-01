using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Models.SportsApiModels
{
    public class Team : BaseSportsApiModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int founded { get; set; }
        public string logo_path { get; set; }
    }
}