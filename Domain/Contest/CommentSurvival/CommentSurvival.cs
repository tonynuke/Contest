using System;
using System.Linq;
using CSharpFunctionalExtensions;

namespace Domain.Contest.CommentSurvival
{
    /// <summary>
    /// Comment survival contest.
    /// </summary>
    /// <remarks>
    /// Participant's comment should be last for <see cref="SurvivalInterval"/>.
    /// </remarks>
    public sealed class CommentSurvival : ContestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSurvival"/> class.
        /// </summary>
        /// <param name="vkGroupId">Vk group id.</param>
        /// <param name="vkPostId">Vk post id.</param>
        /// <param name="configuration">Configuration.</param>
        public CommentSurvival(
            string vkGroupId, long vkPostId, CommentSurvivalConfiguration configuration)
            : base(vkGroupId, vkPostId, configuration)
        {
        }

        private CommentSurvival()
        {
        }

        /// <summary>
        /// Survival start date.
        /// </summary>
        public DateTimeOffset SurvivalStart { get; private set; }

        /// <summary>
        /// Gets contest interval.
        /// </summary>
        public TimeSpan SurvivalInterval => GetConfiguration<CommentSurvivalConfiguration>().SurvivalInterval;

        /// <inheritdoc/>
        public override void Finish(Guid participantId)
        {
            bool isWinnerSurvived = WinnerParticipantIds.Any(id => id == participantId);
            if (isWinnerSurvived)
            {
                Finish();
            }
        }


        /// <inheritdoc/>
        protected override string PlayInternal(Participant participant, string message)
        {
            WinnerParticipantIds.Clear();
            WinnerParticipantIds.Add(participant.Id);
            SurvivalStart = DateTimeOffset.UtcNow;

            return $"Сообщение принято, ждите {SurvivalInterval}";
        }
    }
}