using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Scheduler
{
    /// <summary>
    /// Scheduler interface.
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Schedules method call.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="methodCall">Expression.</param>
        /// <param name="delay">Delay.</param>
        /// <returns>Scheduled job ID.</returns>
        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
    }
}