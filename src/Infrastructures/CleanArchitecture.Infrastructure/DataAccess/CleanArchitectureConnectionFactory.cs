using Microsoft.Data.SqlClient;

namespace CleanArchitecture.Infrastructure.DataAccess;

public class CleanArchitectureConnectionFactory : ICleanArchitectureConnectionFactory
{
    private readonly string _connectionString;

    public CleanArchitectureConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}

public interface ICleanArchitectureConnectionFactory
{
    SqlConnection CreateConnection();
}