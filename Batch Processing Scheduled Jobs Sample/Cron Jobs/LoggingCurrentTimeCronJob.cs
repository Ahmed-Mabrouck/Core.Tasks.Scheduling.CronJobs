using BatchProcessingScheduledJobsSample.Models;
using Core.Tasks.Scheduling.CronJobs;
using Core.Tasks.Scheduling.CronJobs.Configurations.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BatchProcessingScheduledJobsSample.Cron_Jobs
{
    public class LoggingCurrentTimeCronJob : CronJob
    {
        private ILogger<LoggingCurrentTimeCronJob> logger;
        
        public LoggingCurrentTimeCronJob(ISchedulingConfigurations<LoggingCurrentTimeCronJob> configurations, ILogger<LoggingCurrentTimeCronJob> logger) 
            : base(configurations.CronExpression, configurations.TimeZoneInfo)
        {
            this.logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Cron Job Of Type {GetType().Name} With ID {Thread.CurrentThread.ManagedThreadId} Has Started.");
            logger.LogInformation($"Job {Thread.CurrentThread.ManagedThreadId} - Getting Current Time For Cairo Time Zone.");
            await new HttpClient().GetAsync("http://worldtimeapi.org/api/timezone/Africa/Cairo", cancellationToken)
                    .ContinueWith(async t =>
                    {
                        var time = JsonConvert.DeserializeObject<Time>(await t.Result.Content.ReadAsStringAsync());

                        logger.LogInformation($"Job {Thread.CurrentThread.ManagedThreadId} - Current Time Is: {time.datetime}.");
                    }, cancellationToken);
            logger.LogInformation($"Job {Thread.CurrentThread.ManagedThreadId} - Finished Successfully !");
        }
    }
}
