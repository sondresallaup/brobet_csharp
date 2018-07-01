using Brobet.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Brobet.Jobs
{
    public class UpdateFixturesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var apiservice = new SportsApiService();
            apiservice.SaveFixtures(11983); // Eliterien
            apiservice.SaveFixtures(12962); // Premier League
            apiservice.SaveFixtures(12950); // Champions League
            apiservice.SaveFixtures(12945); // Europa League
        }
    }
}