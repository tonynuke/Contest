using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;

namespace Services.Scheduler
{
    /// <summary>
    /// Hangfire scheduler.
    /// </summary>
    public sealed class HangfireScheduler : IScheduler
    {
        /// <inheritdoc/>
        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }
    }
}