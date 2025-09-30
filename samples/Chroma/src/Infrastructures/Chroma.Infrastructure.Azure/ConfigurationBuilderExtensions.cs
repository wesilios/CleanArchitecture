using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace Chroma.Infrastructure.Azure;

public static class ConfigurationBuilderExtensions
{
    public static void AddKeyVault(this IConfigurationBuilder configurationBuilder)
    {
        var configuration = configurationBuilder.Build();
        var enabled = bool.Parse(configuration["Azure:KeyVault:Enabled"] ?? "false");
        if (!enabled)
        {
            return;
        }

        var tenantId = configuration["Azure:TenantId"];
        var clientId = configuration["Azure:ClientId"];
        var clientSecret = configuration["Azure:ClientSecret"];

        var keyVaultUrl = configuration["Azure:KeyVault:Url"];

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        var options = new SecretClientOptions
        {
            Retry =
            {
                Delay = TimeSpan.FromSeconds(5),
                MaxRetries = 5,
            }
        };

        var client = new SecretClient(new Uri(keyVaultUrl), credential, options);

        configurationBuilder.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions
        {
            ReloadInterval = TimeSpan.FromMinutes(1)
        });
    }
}