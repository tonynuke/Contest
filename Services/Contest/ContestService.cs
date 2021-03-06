using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Contest;
using Domain.Contest.CommentSurvival;
using Domain.Contest.SeaBattle;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Scheduler;
using CommentSurvivalConfiguration = Domain.Contest.CommentSurvival.CommentSurvivalConfiguration;
using ContestConfigurationBase = Domain.Contest.ContestConfigurationBase;

namespace Services.Contest
{
    /// <summary>
    /// Contest service.
    /// </summary>
    public class ContestService : IContestService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IScheduler _scheduler;
        private readonly IValidator _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContestService"/> class.
        /// </summary>
        /// <param name="dbContext">Db context.</param>
        /// <param name="scheduler">Scheduler.</param>
        /// <param name="validator">Validator.</param>
        public ContestService(
            ApplicationDbContext dbContext, IScheduler scheduler, IValidator validator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <inheritdoc/>
        public Task<Result<Guid>> CreateContest(
            string vkGroupId,
            long vkPostId,
            ContestType contestType,
            ContestConfigurationBase configuration)
        {
            var newContest = contestType switch
            {
                ContestType.CommentSurvival => new CommentSurvival(
                    vkGroupId, vkPostId, (CommentSurvivalConfiguration)configuration),
                ContestType.SeaBattle => new SeaBattle(vkGroupId, vkPostId, configuration),
                _ => Result.Failure<ContestBase>($"Unknown {contestType}")
            };
            return newContest
                .Tap(contest => _dbContext.AddAsync(contest))
                .Tap(_ => _dbContext.SaveChangesAsync())
                .Tap(ScheduleContestFinish)
                .Map(contest => contest.Id);

            void ScheduleContestFinish(ContestBase contest)
            {
                TimeSpan delay = contest.Configuration.ContestInterval;
                _scheduler.Schedule(
                    (IContestService service) => service.FinishContest(contest.Id), delay);
            }
        }

        /// <inheritdoc/>
        public async Task<string> PlayContest(ContestContext context)
        {
            var currentContest = await _dbContext.Contests
                .Where(contest => contest.VkPostId == context.VkPostId)
                .Include(contest => contest.Participants
                    .Where(participant => participant.VkUserId == context.VkUserId))
                .SingleAsync();

            var validationResult = await _validator.Validate(currentContest, context);
            if (validationResult.IsFailure)
            {
                return validationResult.Error;
            }

            var currentParticipant = currentContest.Participants
                .SingleOrDefault(participant => participant.VkUserId == context.VkUserId);
            bool isParticipantJoinedToContest = currentParticipant != null;
            if (!isParticipantJoinedToContest)
            {
                currentParticipant = currentContest.AddParticipant(
                    context.VkUserId, DateTimeOffset.Now);
            }

            var playResult = currentContest.Play(currentParticipant, context.Message);
            await _dbContext.SaveChangesAsync();

            return playResult;
        }

        /// <inheritdoc/>
        public async Task FinishContest(Guid contestId)
        {
            var contest = await _dbContext.Contests.FindAsync(contestId);
            contest.Finish();
        }

        /// <inheritdoc/>
        public async Task<Maybe<ContestBase>> GetContest(Guid contestId)
        {
            var contestOrNothing = await _dbContext.Contests.FindAsync(contestId);
            return Maybe<ContestBase>.From(contestOrNothing);
        }
    }
}