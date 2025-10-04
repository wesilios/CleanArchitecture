using External.Client.ApiConsumer.Services;
using External.Client.ApiConsumer.Services.Display;
using External.Client.ApiConsumer.Services.Handlers;
using External.Client.ApiConsumer.Services.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace External.Client.ApiConsumer;

/// <summary>
/// Extension methods for service registration
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers console services with HTTP client configuration
    /// </summary>
    public static IServiceCollection AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure HttpClient for API communication
        var apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7001";

        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Register API client services
        services.AddScoped<IPaletteService, PaletteService>();

        return services;
    }

    /// <summary>
    /// Registers all console application services with clean architecture separation
    /// </summary>
    public static IServiceCollection AddCleanConsoleServices(this IServiceCollection services)
    {
        // Display Services
        services.AddScoped<WelcomeDisplayService>();
        services.AddScoped<IMenuDisplayService, MenuDisplayService>();
        services.AddScoped<IPaletteDisplayService, PaletteDisplayService>();

        // Input Services
        services.AddScoped<IPaletteInputService, PaletteInputService>();

        // Command Handlers
        services.AddScoped<IPaletteCommandHandler, PaletteCommandHandler>();

        // Main Application
        services.AddScoped<CleanConsoleApplication>();

        return services;
    }
}