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
            apiservice.UpdateFixtures(11983); // Eliterien
            apiservice.UpdateFixtures(12962); // Premier League
            apiservice.UpdateFixtures(12950); // Champions League
            apiservice.UpdateFixtures(12945); // Europa League
        }
    }
}