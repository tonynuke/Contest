using System;

namespace Domain.Contest
{
    /// <summary>
    /// Base contest configuration.
    /// </summary>
    public abstract class ContestConfigurationBase
    {
        public TimeSpan ContestInterval { get; init; }

        public int? MaxAttemptsCount { get; init; }

        public string MessagePattern { get; init; }
    }
}