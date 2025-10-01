using Chroma.Application.Interfaces;
using Chroma.Domain.Repositories;
using Chroma.Infrastructure.DataAccess;
using Chroma.Infrastructure.DataAccess.Repositories;
using Chroma.Infrastructure.DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chroma.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessServices(configuration);
        services.AddApplicationServices();
    }

    private static void AddApplicationServices(this IServiceCollection services)
    {
        // Register Query Services (the Infrastructure implementations)
        services.AddScoped<IPaletteQueryService, PaletteQueryService>();

        // Register Repositories (the Infrastructure implementations)
        services.AddScoped<IPaletteRepository, PaletteRepository>();
    }

    private static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ChromaDb");

        services.AddDbContextFactory<ChromaDbContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IChromaConnectionFactory, ChromaConnectionFactory>(_ =>
            new ChromaConnectionFactory(connectionString));
    }
}