using System.Data.SqlClient;

namespace Chroma.Infrastructure.DataAccess;

public abstract class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    protected SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}