using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Api;
using Serilog;

[assembly: ExcludeFromCodeCoverage]

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("serilog.json", optional: false, reloadOnChange: true);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

#if (useAzure)
    builder.Configuration.AddAzureKeyVault();
#endif

var configuration = builder.Configuration;
var version = configuration.GetValue<string>("Version");

var logger = Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configurationBuilder.Build())
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

builder.Services.AddApiServices(builder.Configuration);

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"CleanArchitecture Api {version}"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.MapControllers();

app.Run();