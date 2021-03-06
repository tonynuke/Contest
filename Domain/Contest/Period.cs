using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Domain.Contest
{
    /// <summary>
    /// Period.
    /// </summary>
    public class Period : ValueObject
    {
        private Period()
        {
        }

        private Period(DateTimeOffset begin, DateTimeOffset end)
        {
            Begin = begin;
            End = end;
        }

        /// <summary>
        /// Gets period begin date.
        /// </summary>
        public DateTimeOffset Begin { get; }

        /// <summary>
        /// Gets period end date.
        /// </summary>
        public DateTimeOffset End { get; }

        /// <summary>
        /// Creates period.
        /// </summary>
        /// <param name="begin">Begin date.</param>
        /// <param name="end">End date.</param>
        /// <returns>Period.</returns>
        public static Period Create(DateTimeOffset begin, DateTimeOffset end)
        {
            return new (begin, end);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Begin;
            yield return End;
        }
    }
}