using Brobet.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Brobet.Jobs
{
    public class UpdateTodaysFixturesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var apiservice = new SportsApiService();
            //apiservice.UpdateFixturesBetweenDates(DateTime.Now.AddDays(-1).ToString("YYYY-MM-DD"), DateTime.Now.AddDays(1).ToString("YYYY-MM-DD"));
        }
    }
}