namespace Domain.Contest.SeaBattle
{
    /// <summary>
    /// Sea battle contest.
    /// </summary>
    /// <remarks>
    /// Participants should select cells on the fields.
    /// </remarks>
    public sealed class SeaBattle : ContestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeaBattle"/> class.
        /// </summary>
        /// <param name="vkGroupId">Vk group id.</param>
        /// <param name="vkPostId">Vk post id.</param>
        /// <param name="configuration">Configuration.</param>
        public SeaBattle(
            string vkGroupId, long vkPostId, ContestConfigurationBase configuration)
            : base(vkGroupId, vkPostId, configuration)
        {
        }

        private SeaBattle()
        {
        }

        /// <inheritdoc/>
        protected override string PlayInternal(Participant participant, string message)
        {
            throw new System.NotImplementedException();
        }
    }
}