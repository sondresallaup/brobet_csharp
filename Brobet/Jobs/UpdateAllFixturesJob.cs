using Brobet.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Brobet.Jobs
{
    public class UpdateAllFixturesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var apiservice = new SportsApiService();
            apiservice.UpdateAllFixtures(11983); // Eliterien
            apiservice.UpdateAllFixtures(12962); // Premier League
            apiservice.UpdateAllFixtures(12950); // Champions League
            apiservice.UpdateAllFixtures(12945); // Europa League
        }
    }
}