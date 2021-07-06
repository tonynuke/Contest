using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using RT.Comb;

namespace Domain.Contest
{
    /// <summary>
    /// Base contest.
    /// </summary>
    public abstract class ContestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContestBase"/> class.
        /// </summary>
        /// <param name="vkGroupId">Vk group id.</param>
        /// <param name="vkPostId">Vk post id.</param>
        /// <param name="configuration">Configuration.</param>
        protected ContestBase(
            string vkGroupId,
            long vkPostId,
            ContestConfigurationBase configuration)
        {
            Id = Provider.PostgreSql.Create();
            VkGroupId = vkGroupId;
            VkPostId = vkPostId;
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContestBase"/> class.
        /// </summary>
        protected ContestBase()
        {
        }

        /// <summary>
        /// Gets id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets vk group id.
        /// </summary>
        public string VkGroupId { get; }

        /// <summary>
        /// Gets vk post id.
        /// </summary>
        public long VkPostId { get; }

        /// <summary>
        /// Gets participants.
        /// </summary>
        public IList<Participant> Participants { get; } = new List<Participant>();

        /// <summary>
        /// Gets or sets winner participants ids.
        /// </summary>
        public IList<Guid> WinnerParticipantIds { get; protected set; } = new List<Guid>();

        /// <summary>
        /// Gets a value indicating whether contest is finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Gets base contest configuration.
        /// </summary>
        public ContestConfigurationBase Configuration { get; }

        /// <summary>
        /// Gets a value indicating whether contest is finished.
        /// </summary>
        protected virtual bool IsFinishedInternal => true;

        /// <summary>
        /// Gets contest configuration of specific type.
        /// </summary>
        /// <typeparam name="TContestConfiguration">Contest configuration type.</typeparam>
        /// <returns>Contest configuration.</returns>
        public TContestConfiguration GetConfiguration<TContestConfiguration>()
            where TContestConfiguration : ContestConfigurationBase
        {
            return (TContestConfiguration)Configuration;
        }

        /// <summary>
        /// Plays contest.
        /// </summary>
        /// <param name="participant">Participant.</param>
        /// <param name="message">Message.</param>
        /// <returns>Play result.</returns>
        public Result<IReadOnlyCollection<PlayResult>> Play(Participant participant, string message)
        {
            return participant.TryMakeAttempt()
                ? PlayInternal(participant, message)
                : Result.Failure<IReadOnlyCollection<PlayResult>>("Попытки кончились");
        }

        /// <summary>
        /// Finishes contest.
        /// </summary>
        public void Finish()
        {
            IsFinished = IsFinishedInternal;
        }

        /// <summary>
        /// Adds participant to contest.
        /// </summary>
        /// <param name="vkUserId">Participant vk id.</param>
        /// <param name="vkPeerId">Participant vk peer id.</param>
        /// <param name="lastCommentDate">Last comment date.</param>
        /// <returns>Participant.</returns>
        public Participant AddParticipant(long vkUserId, long vkPeerId, DateTimeOffset lastCommentDate)
        {
            var participant = new Participant(
                Id, vkUserId, vkPeerId, Configuration.MaxAttemptsCount, lastCommentDate);
            Participants.Add(participant);
            return participant;
        }

        /// <summary>
        /// Plays contest internal.
        /// </summary>
        /// <param name="participant">Participant.</param>
        /// <param name="message">Message.</param>
        /// <returns>Play result.</returns>
        protected abstract Result<IReadOnlyCollection<PlayResult>> PlayInternal(Participant participant, string message);
    }

    public readonly struct PlayResult
    {
        public Participant Participant { get; init; }

        public string Message { get; init; }
    }
}