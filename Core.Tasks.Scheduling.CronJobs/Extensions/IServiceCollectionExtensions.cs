using Core.Tasks.Scheduling.CronJobs.Configurations;
using Core.Tasks.Scheduling.CronJobs.Configurations.Base;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Tasks.Scheduling.CronJobs.Extensions
{
    /// <summary>
    /// Extensions for IServiceCollection interface to add a cron job to DI container.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<ISchedulingConfigurations<T>> options) where T : CronJob
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }

            var config = new DefaultSchedulingConfigurations<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(DefaultSchedulingConfigurations<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<ISchedulingConfigurations<T>>(config);
            services.AddHostedService<T>();
            return services;
        }
    }
}
