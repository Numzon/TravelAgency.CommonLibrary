using Amazon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TravelAgency.SharedLibrary.AWS;
public static class SecretManagerConfiguration
{
    public static IConfigurationBuilder AddAndConfigureSecretManager(this IConfigurationBuilder configuration, IWebHostEnvironment environment, RegionEndpoint region, string? prefix = null)
    {
        if (region is null)
        {
            throw new InvalidOperationException("RegionEndpoint cannot be null");
        }

        configuration.AddSecretsManager(
           region: region,
           configurator: o =>
           {
               string secretPrefix = $"{prefix ?? string.Empty}{environment.EnvironmentName}_{environment.ApplicationName.Replace(".", "_")}_";
               o.SecretFilter = s => s.Name
                   .StartsWith(secretPrefix, StringComparison.OrdinalIgnoreCase);

               o.KeyGenerator = (_, s) => s
                   .Replace(secretPrefix, string.Empty, StringComparison.OrdinalIgnoreCase)
                   .Replace("__", ":", StringComparison.Ordinal);
           });

        return configuration;
    }
}
