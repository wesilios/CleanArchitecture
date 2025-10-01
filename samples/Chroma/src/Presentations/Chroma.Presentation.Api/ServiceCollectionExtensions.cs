using System.Text.Json.Serialization;
using Chroma.Domain.Constants;
using Chroma.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Chroma.Presentation.Api;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddCors();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        services.AddInfrastructureServices(configuration);

        services.Configure<ApiBehaviorOptions>(options => { });

        services.AddSwaggerGen(options =>
        {
            var version = configuration.GetValue<string>("Version");

            options.SwaggerDoc($"v{version}",
                new OpenApiInfo { Title = "Chroma Api", Version = $"v{version}" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "JWT Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer,
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = OidcConstants.AuthenticationSchemes.AuthorizationHeaderBearer
                        }
                    },
                    Array.Empty<string>()
                }
            });
            options.EnableAnnotations();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name?.StartsWith("Chroma") ?? false);
            foreach (var assembly in assemblies)
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            }
        });
    }
}