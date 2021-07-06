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
        public void When_SurvivalIntervalIsNotOver_Contest_Should_Be_Finished()
        {
            var participant = _fixture.Create<Participant>();
            var message = _fixture.Create<string>();
            var contest = Create(0);
            contest.WinnerParticipantIds.Should().BeEmpty();

            contest.Play(participant, message);

            contest.WinnerParticipantIds.Should().Contain(participant.Id);
            contest.IsFinished.Should().BeFalse();

            contest.Finish();
            contest.IsFinished.Should().BeTrue();
        }

        [Fact]
        public void When_SurvivalIntervalIsOver_Contest_Should_Not_Be_Finished()
        {
            var participant = _fixture.Create<Participant>();
            var message = _fixture.Create<string>();
            var contest = Create(1_000_000);
            contest.WinnerParticipantIds.Should().BeEmpty();

            contest.Play(participant, message);

            contest.WinnerParticipantIds.Should().Contain(participant.Id);
            contest.IsFinished.Should().BeFalse();

            contest.Finish();
            contest.IsFinished.Should().BeFalse();
        }

        private CommentSurvival Create(int milliseconds)
        {
            var vkGroupId = _fixture.Create<string>();
            var vkPostId = _fixture.Create<long>();
            var commentSurvivalConfiguration = new CommentSurvivalConfiguration()
            {
                SurvivalInterval = TimeSpan.FromMilliseconds(milliseconds)
            };
            return new CommentSurvival(vkGroupId, vkPostId, commentSurvivalConfiguration);
        }
    }
}