using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Tasks.Scheduling.CronJobs
{
    /// <summary>
    /// Abstract base class for cron jobs.
    /// </summary>
    public abstract class CronJob : IHostedService, IDisposable
    {
        /// <summary>
        /// Scheduling timer.
        /// </summary>
        private System.Timers.Timer timer;
        /// <summary>
        /// Cron expression for scheduling.
        /// </summary>
        private readonly CronExpression cronExpression;
        /// <summary>
        /// Custom timezone information.
        /// </summary>
        private readonly TimeZoneInfo timeZoneInfo;

        /// <summary>
        /// Constructor for supplying mandatory parameters.
        /// </summary>
        /// <param name="cronExpression">Cron expression for secheduling job.</param>
        /// <param name="timeZoneInfo">Custom timezone information.</param>
        protected CronJob(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            this.cronExpression = CronExpression.Parse(cronExpression);
            this.timeZoneInfo = timeZoneInfo;
        }

        /// <summary>
        /// Schedule the job according to supplied cron expression and timezone.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the job task execution.</param>
        /// <returns></returns>
        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = cronExpression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed += async (sender, args) =>
                {
                    timer.Stop();
                    await ExecuteAsync(cancellationToken);
                    await ScheduleJob(cancellationToken);
                };
                timer.Start();
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the job task execution.</param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            timer?.Dispose();
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
