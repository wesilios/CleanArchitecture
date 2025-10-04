using External.Client.ApiConsumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace External.Client.ApiConsumer;

public static class ServiceCollectionExtensions
{
    public static void AddConsoleServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure HttpClient for API communication
        var apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7001";

        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Register application services
        services.AddScoped<ConsoleApplication>();
        services.AddScoped<IPaletteService, PaletteService>();
        services.AddScoped<IUserInterface, ConsoleUserInterface>();
    }
}