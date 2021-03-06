using System;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace UnitTests
{
    /// <summary>
    /// Unit test utils.
    /// </summary>
    public static class UnitTestUtils
    {
        /// <summary>
        /// Creates in memory database context.
        /// </summary>
        /// <typeparam name="TContext">DbContext type.</typeparam>
        /// <param name="databaseName">Database name.</param>
        /// <returns>DbContext.</returns>
        public static TContext CreateInMemoryDbContext<TContext>(string databaseName)
            where TContext : DbContext
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return (TContext)Activator.CreateInstance(typeof(TContext), dbContextOptions);
        }
    }
}