using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Contest;
using FluentAssertions;
using Moq;
using Persistence;
using Services;
using Services.Contest;
using Services.Scheduler;
using Xunit;
using CommentSurvivalConfiguration = Domain.Contest.CommentSurvival.CommentSurvivalConfiguration;

namespace IntegrationTests
{
    public class TestScheduler : IScheduler
    {
        private object _func;

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            _func = methodCall.Compile();
            return Guid.NewGuid().ToString();
        }

        public void Fire<T>(T owner)
        {
            ((Func<T, Task>)_func).Invoke(owner);
        }
    }

    public class ContestServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _databaseFixture;

        private readonly ApplicationDbContext _dbContext;

        private readonly IContestService _contestService;

        private readonly TestScheduler _scheduler = new ();

        private readonly Mock<IValidator> _validatorMock = new ();

        private readonly Fixture _fixture = new ();

        public ContestServiceTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture ?? throw new ArgumentNullException(nameof(databaseFixture));
            _dbContext = new ApplicationDbContext(_databaseFixture.DbContextOptions);
            _contestService = new ContestService(_dbContext, _scheduler, _validatorMock.Object);
        }

        [Fact]
        public async Task CreateContest()
        {
            string vkGroupId = _fixture.Create<string>();
            long vkPostId = _fixture.Create<long>();
            long vkUserId = _fixture.Create<long>();
            string message = _fixture.Create<string>();
            var creationResult = await _contestService.CreateContest(
                vkGroupId, vkPostId, ContestType.CommentSurvival, new CommentSurvivalConfiguration());
            creationResult.IsSuccess.Should().BeTrue();

            var contest = await _contestService.GetContest(creationResult.Value);
            contest.HasValue.Should().BeTrue();
            contest.Value.Participants.Should().BeEmpty();
            contest.Value.WinnerParticipantIds.Should().BeEmpty();

            var participantContext1 = new ContestContext(vkPostId, vkUserId, message);
            await _contestService.PlayContest(participantContext1);

            var participantContext2 = new ContestContext(vkPostId, _fixture.Create<long>(), _fixture.Create<string>());
            await _contestService.PlayContest(participantContext2);

            contest.Value.Participants.Should().NotBeEmpty();
            contest.Value.WinnerParticipantIds.Should().NotBeEmpty();
            contest.Value.IsFinished.Should().BeFalse();

            _scheduler.Fire(_contestService);

            contest.Value.IsFinished.Should().BeTrue();
            contest.Value.Participants.Count.Should().Be(2);
        }
    }
}