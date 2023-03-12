using System.Data;

namespace CleanArchitectureTemplate.Domain.Common.Database;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}