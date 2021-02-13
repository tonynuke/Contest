namespace Common
{
    public class DatabaseConfiguration
    {
        public const string Key = "DbConfig";

        public string ConnectionString { get; set; }

        public bool LogQueries { get; set; }

        public bool ApplyMigrationsOnStartup { get; set; }
    }
}