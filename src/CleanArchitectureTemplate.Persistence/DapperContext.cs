using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Data;

namespace CleanArchitectureTemplate.Persistence
{
    public class DapperContext : IDapperContext
    {
        private readonly IOptions<DatabaseOptions> _options;

        public DapperContext(IOptions<DatabaseOptions> options)
        {
            _options = options;
        }

        public IDbConnection CreateConnection()
            => new SqliteConnection(_options.Value.Connection);
    }
}