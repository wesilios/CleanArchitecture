namespace CleanArchitecture.Infrastructure.DataAccess;


public class ApplicationConnectionFactory : SqlConnectionFactory, IApplicationConnectionFactory
{
    public ApplicationConnectionFactory(string connectionString) : base(connectionString)
    {
    }
}

public interface IApplicationConnectionFactory : ISqlConnectionFactory
{
}