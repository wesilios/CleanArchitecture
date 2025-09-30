using Chroma.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chroma.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ChromaDb");
        
        services.AddDbContextFactory<ChromaDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        services.AddScoped<IChromaConnectionFactory, ChromaConnectionFactory>(_ =>
            new ChromaConnectionFactory(connectionString));
    }
}