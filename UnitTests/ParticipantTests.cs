using System;
using Domain;
using FluentAssertions;
using Xunit;

namespace UnitTests
{
    public class ParticipantTests
    {
        [Fact]
        public void TryMakeAttempt_When_MaxAttemptsNotSpecified_ShouldBe_Success()
        {
            var participant = new Participant(Guid.NewGuid(), 1, 2, null, DateTimeOffset.Now);
            participant.ActualAttemptsCount.Should().BeNull();
            participant.TryMakeAttempt().Should().BeTrue();
            participant.ActualAttemptsCount.Should().BeNull();
        }

        [Fact]
        public void TryMakeAttempt_When_MaxAttemptsIsSpecified_ShouldBe_Success()
        {
            var participant = new Participant(Guid.NewGuid(), 1, 2, 3, DateTimeOffset.Now);
            participant.ActualAttemptsCount.Should().Be(1);
            participant.TryMakeAttempt().Should().BeTrue();
            participant.ActualAttemptsCount.Should().Be(2);
        }

        [Fact]
        public void TryMakeAttempt_When_AttemptsIsOver_ShouldBe_Failure()
        {
            var participant = new Participant(Guid.NewGuid(), 3, 2, 1, DateTimeOffset.Now);
            participant.ActualAttemptsCount.Should().Be(1);
            participant.TryMakeAttempt().Should().BeFalse();
            participant.ActualAttemptsCount.Should().Be(1);
        }
    }
}