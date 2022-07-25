using System;

namespace CleanArchitectureTemplate.Persistence.Settings.Options
{
    public sealed class DatabaseOptions
    {
        public static readonly string Database = "Database";

        public string Connection { get; set; } = String.Empty;
    }
}