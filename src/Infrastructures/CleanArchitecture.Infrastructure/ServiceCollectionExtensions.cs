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
        
        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
#if (UseSqlite)
            options.UseSqlite(connectionString);
#else
            options.UseSqlServer(connectionString);
#endif
        });
        
        services.AddScoped<IApplicationConnectionFactory, ApplicationConnectionFactory>(_ =>
            new ApplicationConnectionFactory(connectionString));
    }
}