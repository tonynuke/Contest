using System;

namespace Domain.Contest.CommentSurvival
{
    /// <summary>
    /// Comment survival contest configuration.
    /// </summary>
    public class CommentSurvivalConfiguration : ContestConfigurationBase
    {
        /// <summary>
        /// Gets contest interval.
        /// </summary>
        public TimeSpan SurvivalInterval { get; init; }
    }
}