using Microsoft.Extensions.DependencyInjection;
using TravelAgency.SharedLibrary.Vault.Consts;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace TravelAgency.SharedLibrary.Vault;
public static class VaultConfiguration
{
    /// <summary>
    /// Requires three environment variables to work properly 
    /// HASHI_CORP_VAULT_TOKEN / HASHI_CORP_VAULT_HOST / HASHI_CORP_VAULT_PORT
    /// </summary>
    /// <param name="services"></param>
    /// <param name="ssl">http or https protocol</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">If environments weren't defined exception is thrown</exception>
    public static IServiceCollection ConfigureVault(this IServiceCollection services, bool ssl = false)
    {
        var token = Environment.GetEnvironmentVariable(VaultEnvironmentVariables.Token) ?? throw new ArgumentException("Vault token is null");
        var host = Environment.GetEnvironmentVariable(VaultEnvironmentVariables.Host) ?? throw new ArgumentException("Vault host is null");
        var port = Environment.GetEnvironmentVariable(VaultEnvironmentVariables.Port) ?? throw new ArgumentException("Vault port is null");
        var protocol = ssl ? "https" : "http";

        IAuthMethodInfo authMethod = new TokenAuthMethodInfo(token);
        var vaultClientSettings = new VaultClientSettings($"{protocol}://{host}:{port}", authMethod);

        var vaultClient = new VaultClient(vaultClientSettings);

        services.AddSingleton<IVaultClient>(vaultClient);

        return services;
    }
}
