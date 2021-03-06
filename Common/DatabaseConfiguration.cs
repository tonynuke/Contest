namespace Common
{
    /// <summary>
    /// Database configuration.
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Configuration key.
        /// </summary>
        public const string Key = "DbConfig";

        /// <summary>
        /// Gets or sets connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log queries.
        /// </summary>
        public bool LogQueries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether apply migrations on startup.
        /// </summary>
        public bool ApplyMigrationsOnStartup { get; set; }
    }
}