using System.Collections.Generic;

namespace Domain.Contest.SeaBattle
{
    /// <summary>
    /// Sea battle contest configuration.
    /// </summary>
    public class SeaBattleConfiguration : ContestConfigurationBase
    {
        /// <summary>
        /// Gets field size.
        /// </summary>
        public int FieldSize { get; init; }

        /// <summary>
        /// Gets prizes.
        /// </summary>
        public IReadOnlyCollection<string> Prizes { get; init; }
    }
}