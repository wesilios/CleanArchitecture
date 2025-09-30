using CleanArchitecture.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CleanArchitectureDb");
        
        services.AddDbContextFactory<CleanArchitectureDbContext>(options =>
        {
#if (useSqlite)
            options.UseSqlite(connectionString);
#else
            options.UseSqlServer(connectionString);
#endif
        });
        
        services.AddScoped<ICleanArchitectureConnectionFactory, CleanArchitectureConnectionFactory>(_ =>
            new CleanArchitectureConnectionFactory(connectionString));
    }
}