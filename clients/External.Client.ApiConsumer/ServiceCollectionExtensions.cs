using External.Client.ApiConsumer.Services;
using External.Client.ApiConsumer.Services.Display;
using External.Client.ApiConsumer.Services.Handlers;
using External.Client.ApiConsumer.Services.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;
using External.Client.ApiConsumer.Services.HttpClients;

namespace External.Client.ApiConsumer;

/// <summary>
/// Extension methods for service registration
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers console services with Refit API client configuration
    /// </summary>
    public static void AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Refit API client
        var apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7001";

        // Configure Refit settings
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
        };

        // Register Refit API interface
        services.AddRefitClient<IPaletteApiClient>(refitSettings)
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

        // Register API client services
        services.AddScoped<IApiClientService, PaletteApiClientService>();
        services.AddScoped<IPaletteService, PaletteService>();
    }

    /// <summary>
    /// Registers all console application services with clean architecture separation
    /// </summary>
    public static void AddCleanConsoleServices(this IServiceCollection services)
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
        services.AddScoped<ConsoleApplication>();
    }
}