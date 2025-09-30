namespace Chroma.Infrastructure.DataAccess;

public class ChromaConnectionFactory : SqlConnectionFactory, IChromaConnectionFactory
{
    public ChromaConnectionFactory(string connectionString) : base(connectionString)
    {
    }
}

public interface IChromaConnectionFactory : ISqlConnectionFactory
{
}