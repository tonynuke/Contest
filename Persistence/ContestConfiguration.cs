using Domain.Contest;
using Domain.Contest.CommentSurvival;
using Domain.Contest.SeaBattle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence
{
    public class ContestConfiguration : IEntityTypeConfiguration<ContestBase>
    {
        public void Configure(EntityTypeBuilder<ContestBase> builder)
        {
            builder.HasDiscriminator();

            builder.HasKey(contest => contest.Id);
            builder.Property(contest => contest.Id).ValueGeneratedNever();

            builder.Property(contest => contest.VkPostId);
            builder.Property(contest => contest.Configuration).HasColumnType("jsonb");
            builder.Property(contest => contest.IsFinished);
            builder.Property(contest => contest.WinnerParticipantIds).HasColumnType("jsonb");
            builder.HasMany(contest => contest.Participants).WithOne().HasForeignKey(participant => participant.Id);
        }
    }

    public class CommentSurvivalConfiguration : IEntityTypeConfiguration<CommentSurvival>
    {
        public void Configure(EntityTypeBuilder<CommentSurvival> builder)
        {
            builder.HasBaseType<ContestBase>();
            builder.Property(contest => contest.SurvivalEnd);
        }
    }

    public class SeaBattleConfiguration : IEntityTypeConfiguration<SeaBattle>
    {
        public void Configure(EntityTypeBuilder<SeaBattle> builder)
        {
            builder.HasBaseType<ContestBase>();
        }
    }
}