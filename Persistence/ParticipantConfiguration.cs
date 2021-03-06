using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(participant => participant.Id);
            builder.Property(contest => contest.Id).ValueGeneratedNever();

            builder.Property(participant => participant.VkUserId);
            builder.Property(participant => participant.MaxAttemptsCount);
            builder.Property(participant => participant.ActualAttemptsCount);
            builder.Property(participant => participant.LastCommentDate);
            builder.Property(participant => participant.IsWinner);
            builder.Property(participant => participant.ContestId);
        }
    }
}