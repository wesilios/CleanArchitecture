using External.Client.ApiConsumer;
using External.Client.ApiConsumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = Host.CreateDefaultBuilder(args).UseSerilog();

builder.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
        optional: true, reloadOnChange: true);
    config.AddEnvironmentVariables();
    config.AddCommandLine(args);
});

builder.ConfigureServices((context, services) =>
{
    services.AddConsoleServices(context.Configuration);
    services.AddCleanConsoleServices();
});

try
{
    Log.Information("Starting CleanArchitecture Console Application with Clean Architecture");

    var host = builder.Build();

    // Use the new clean console application
    var app = host.Services.GetRequiredService<ConsoleApplication>();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}