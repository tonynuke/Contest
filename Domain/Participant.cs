using System;
using RT.Comb;

namespace Domain
{
    /// <summary>
    /// Participant.
    /// </summary>
    public class Participant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Participant"/> class.
        /// </summary>
        /// <param name="contestId">Contest Id.</param>
        /// <param name="vkUserId">Vk user Id.</param>
        /// <param name="vkPeerId">Vk peer Id.</param>
        /// <param name="maxAttemptsCount">Max attempts count.</param>
        /// <param name="lastCommentDate">Last comment date.</param>
        public Participant(
            Guid contestId,
            long vkUserId,
            long vkPeerId,
            int? maxAttemptsCount,
            DateTimeOffset lastCommentDate)
        {
            Id = Provider.PostgreSql.Create();
            ContestId = contestId;
            VkUserId = vkUserId;
            VkPeerId = vkPeerId;
            MaxAttemptsCount = maxAttemptsCount;
            LastCommentDate = lastCommentDate;
            ActualAttemptsCount = maxAttemptsCount.HasValue ? 1 : null;
        }

        private Participant()
        {
        }

        /// <summary>
        /// Gets ID.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets contest ID.
        /// </summary>
        public Guid ContestId { get; }

        /// <summary>
        /// Gets vk user ID.
        /// </summary>
        public long VkUserId { get; }

        /// <summary>
        /// Gets or sets vk peer ID.
        /// </summary>
        public long VkPeerId { get; set; }

        /// <summary>
        /// Gets max attempts count.
        /// </summary>
        public int? MaxAttemptsCount { get; }

        /// <summary>
        /// Gets actual attempts count.
        /// </summary>
        public int? ActualAttemptsCount { get; private set; }

        /// <summary>
        /// Gets last comment date.
        /// </summary>
        public DateTimeOffset LastCommentDate { get; }

        /// <summary>
        /// Gets or sets a value indicating whether participant is winner.
        /// </summary>
        public bool IsWinner { get; set; }

        /// <summary>
        /// Tries to make attempt.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if attempt is successful, otherwise <see langword="false"/>.
        /// </returns>
        public bool TryMakeAttempt()
        {
            if (ActualAttemptsCount >= MaxAttemptsCount)
            {
                return false;
            }

            if (MaxAttemptsCount.HasValue)
            {
                ActualAttemptsCount++;
            }

            return true;
        }
    }
}