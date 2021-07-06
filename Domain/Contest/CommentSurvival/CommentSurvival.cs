using System;
using System.Collections.Generic;
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
        /// Gets survival end date.
        /// </summary>
        public DateTimeOffset SurvivalEnd { get; private set; }

        /// <summary>
        /// Gets contest interval.
        /// </summary>
        public TimeSpan SurvivalInterval => GetConfiguration<CommentSurvivalConfiguration>().SurvivalInterval;

        public Participant CurrentWinner { get; private set; }

        /// <inheritdoc/>
        protected override bool IsFinishedInternal => SurvivalEnd < DateTimeOffset.UtcNow;

        /// <inheritdoc/>
        protected override Result<IReadOnlyCollection<PlayResult>> PlayInternal(Participant participant, string message)
        {
            var playResult = new List<PlayResult>();
            if (WinnerParticipantIds.Any())
            {
                var result = new PlayResult
                {
                    Participant = CurrentWinner,
                    Message = "Вы больше не лидер."
                };
                playResult.Add(result);
            }

            CurrentWinner = participant;
            WinnerParticipantIds.Clear();
            WinnerParticipantIds.Add(participant.Id);
            SurvivalEnd = DateTimeOffset.UtcNow.Add(SurvivalInterval);
            playResult.Add(new PlayResult()
            {
                Participant = CurrentWinner,
                Message = $"Осталось продержаться до {SurvivalEnd}."
            });

            return playResult;
        }
    }
}