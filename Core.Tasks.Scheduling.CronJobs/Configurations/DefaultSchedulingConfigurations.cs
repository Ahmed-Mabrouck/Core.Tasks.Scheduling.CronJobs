using Core.Tasks.Scheduling.CronJobs.Configurations.Base;
using System;

namespace Core.Tasks.Scheduling.CronJobs.Configurations
{
    public class DefaultSchedulingConfigurations<T> : ISchedulingConfigurations<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
