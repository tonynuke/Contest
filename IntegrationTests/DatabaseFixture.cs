using System;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; }

        public DbContextOptions<ApplicationDbContext> DbContextOptions { get; }

        public DatabaseFixture()
        {
            DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost; Database=Integration; Username=postgres; Password=mysecretpassword; Application Name=Contest; Pooling=true;")
                .Options;

            DbContext = new ApplicationDbContext(DbContextOptions);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
        }
    }
}