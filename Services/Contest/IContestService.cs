using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Contest;

namespace Services.Contest
{
    /// <summary>
    /// Contest service interface.
    /// </summary>
    public interface IContestService
    {
        /// <summary>
        /// Creates contest.
        /// </summary>
        /// <param name="vkGroupId">Vk group id.</param>
        /// <param name="vkPostId">Vk post id.</param>
        /// <param name="contestType">Contest type.</param>
        /// <param name="configuration">Contest configuration.</param>
        /// <returns>Contest ID.</returns>
        public Task<Result<Guid>> CreateContest(
            string vkGroupId,
            long vkPostId,
            ContestType contestType,
            ContestConfigurationBase configuration);

        /// <summary>
        /// Plays contest.
        /// </summary>
        /// <param name="context">Contest context.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task<string> PlayContest(ContestContext context);

        /// <summary>
        /// Finishes contest.
        /// </summary>
        /// <param name="contestId">Contest Id.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task FinishContest(Guid contestId);

        /// <summary>
        /// Gets contest.
        /// </summary>
        /// <param name="contestId">Contest Id.</param>
        /// <returns>Contest or nothing.</returns>
        public Task<Maybe<ContestBase>> GetContest(Guid contestId);
    }
}