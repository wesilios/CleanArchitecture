using Chroma.Application;
using Chroma.Application.Commands;
using Chroma.Application.Common;
using Chroma.Application.DataObjects;
using Chroma.Application.Handlers;
using Chroma.Application.Handlers.Abstractions;
using Chroma.Application.Interfaces;
using Chroma.Application.Queries;
using Chroma.Domain.Repositories;
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
        var connectionString = configuration.GetConnectionString("ChromaDb");

        services.AddDbContextFactory<ChromaDbContext>(options => { options.UseSqlServer(connectionString); });

        services.AddScoped<IChromaConnectionFactory, ChromaConnectionFactory>(_ =>
            new ChromaConnectionFactory(connectionString));
    }
}