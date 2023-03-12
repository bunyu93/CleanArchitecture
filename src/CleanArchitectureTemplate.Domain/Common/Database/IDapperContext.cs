using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Database;

public interface IDapperContext
{
    IDbConnection CreateConnection();

    Task<IEnumerable<TResult>> QueryHandler<TResult>(string query);

    Task<IEnumerable<TResult>> QueryHandler<TResult>(string query, object param);

    Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query);

    Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query, object param);
}