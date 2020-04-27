using System;

namespace Core.Tasks.Scheduling.CronJobs.Configurations.Base
{
    public interface ISchedulingConfigurations<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
