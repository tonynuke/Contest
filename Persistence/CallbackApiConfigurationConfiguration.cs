using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence
{
    public class CallbackApiConfigurationConfiguration : IEntityTypeConfiguration<CallbackApiConfiguration>
    {
        public void Configure(EntityTypeBuilder<CallbackApiConfiguration> builder)
        {
            builder.HasKey(configuration => configuration.Id);

            builder.Property(configuration => configuration.AccessToken);
            builder.Property(configuration => configuration.ConfirmationKey);
            builder.Property(configuration => configuration.GroupId);

            builder.HasIndex(configuration => configuration.GroupId);
        }
    }
}