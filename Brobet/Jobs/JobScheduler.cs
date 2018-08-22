using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Brobet.Jobs
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail allFixturesJob = JobBuilder.Create<UpdateAllFixturesJob>().Build();
            ITrigger allFixturesTrigger = TriggerBuilder.Create()
            .WithIdentity("allFixturestrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(86400)
            .RepeatForever())
            .Build();
            scheduler.ScheduleJob(allFixturesJob, allFixturesTrigger);

            IJobDetail todaysFixturesJob = JobBuilder.Create<UpdateTodaysFixturesJob>().Build();
            ITrigger todaysFixturesTrigger = TriggerBuilder.Create()
            .WithIdentity("todaysFixturesTrigger", "group2")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(600)
            .RepeatForever())
            .Build();
            scheduler.ScheduleJob(todaysFixturesJob, todaysFixturesTrigger);
        }
    }
}