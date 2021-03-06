using System.Reflection;
using Domain;
using Domain.Contest;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    /// <summary>
    /// Application Db context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">Db context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets callback api configurations.
        /// </summary>
        public DbSet<CallbackApiConfiguration> CallbackApiConfigurations { get; set; }

        /// <summary>
        /// Gets or sets contests.
        /// </summary>
        public DbSet<ContestBase> Contests { get; set; }

        /// <summary>
        /// Gets or sets participants.
        /// </summary>
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}