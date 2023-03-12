using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.EntityFramework;

public class DapperDbContext : IDapperContext
{
    private readonly IOptions<DatabaseOptions> _options;

    public DapperDbContext(IOptions<DatabaseOptions> options)
    {
        _options = options;
    }

    public IDbConnection CreateConnection()
        => new SqliteConnection(_options.Value.Connection);

    public async Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query)
    {
        var dbContext = CreateConnection();
        dbContext.Open();
        var result = await dbContext.QueryFirstOrDefaultAsync<TResult>(query) ?? throw new NotFoundException();
        dbContext.Close();

        return result;
    }

    public async Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query, object param)
    {
        var dbContext = CreateConnection();
        dbContext.Open();
        var result = await dbContext.QueryFirstOrDefaultAsync<TResult>(query, param) ?? throw new NotFoundException(JsonSerializer.Serialize(param));
        dbContext.Close();

        return result;
    }

    public async Task<IEnumerable<TResult>> QueryHandler<TResult>(string query)
    {
        var dbContext = CreateConnection();
        dbContext.Open();
        var result = await dbContext.QueryAsync<TResult>(query) ?? throw new NotFoundException();
        dbContext.Close();

        return result;
    }

    public async Task<IEnumerable<TResult>> QueryHandler<TResult>(string query, object param)
    {
        var dbContext = CreateConnection();
        dbContext.Open();
        var result = await dbContext.QueryAsync<TResult>(query, param) ?? throw new NotFoundException(JsonSerializer.Serialize(param));
        dbContext.Close();

        return result;
    }
}