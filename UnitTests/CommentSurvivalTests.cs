using System;
using AutoFixture;
using Domain;
using Domain.Contest.CommentSurvival;
using FluentAssertions;
using Xunit;

namespace UnitTests
{
    public class CommentSurvivalTests
    {
        private readonly Fixture _fixture = new ();

        [Fact]
        public void CreateContest()
        {
            var vkGroupId = _fixture.Create<string>();
            var vkPostId = _fixture.Create<long>();
            var contestConfiguration = _fixture.Create<CommentSurvivalConfiguration>();

            var contest = new CommentSurvival(vkGroupId, vkPostId, contestConfiguration);

            contest.WinnerParticipantIds.Should().BeEmpty();
            contest.Participants.Should().BeEmpty();
            contest.SurvivalInterval.Should().Be(contestConfiguration.SurvivalInterval);
        }

        [Fact]
        public void PlayContest_Should_Be_Finished()
        {
            var participant = _fixture.Create<Participant>();
            var message = _fixture.Create<string>();
            var contest = _fixture.Create<CommentSurvival>();
            contest.WinnerParticipantIds.Should().BeEmpty();

            contest.Play(participant, message);

            contest.WinnerParticipantIds.Should().Contain(participant.Id);
            contest.IsFinished.Should().BeFalse();

            contest.Finish(participant.Id);
            contest.IsFinished.Should().BeTrue();
        }

        [Fact]
        public void PlayContest_ShouldNot_Be_Finished()
        {
            var participant = _fixture.Create<Participant>();
            var message = _fixture.Create<string>();
            var contest = _fixture.Create<CommentSurvival>();
            contest.WinnerParticipantIds.Should().BeEmpty();

            contest.Play(participant, message);

            contest.WinnerParticipantIds.Should().Contain(participant.Id);
            contest.IsFinished.Should().BeFalse();

            contest.Finish(Guid.NewGuid());
            contest.IsFinished.Should().BeFalse();
        }
    }
}