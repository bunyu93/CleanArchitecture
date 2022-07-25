using System.Data;

namespace CleanArchitectureTemplate.Domain.Common
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}