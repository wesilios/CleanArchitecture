using CleanArchitecture.Application;
using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.DataObjects;
using CleanArchitecture.Application.Handlers;
using CleanArchitecture.Application.Handlers.Abstractions;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries;
using CleanArchitecture.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

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
        
        // Register the Dispatcher
        services.AddScoped<IDispatcher, Dispatcher>();
        
        // Register Query/Command Handlers
        services.AddTransient<ICommandHandler<AddColorToPaletteCommand>, AddColorToPaletteHandler>();
        services.AddTransient<ICommandHandler<AddPaletteCommand>, AddPaletteCommandHandler>();
        services.AddTransient<IQueryHandler<GetAllPalettesQuery, IPagedList<IPaletteDto>>, GetAllPalettesQueryHandler>();
        services.AddTransient<IQueryHandler<GetPaletteByIdQuery, IPaletteDto>, GetPaletteByIdQueryHandler>();
    }

    private static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
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