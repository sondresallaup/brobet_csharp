using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brobet.Models.SportsApiModels
{
    public class Season : BaseSportsApiModel
    {
        public SeasonData data { get; set; }

        public class SeasonData
        {
            public DataObject<Fixture> fixtures { get; set; }
        }
    }
}