using TravelAgency.SharedLibrary.Vault.Consts;
using VaultSharp;

namespace TravelAgency.SharedLibrary.Vault;
public class VaultFacade
{
    private readonly IVaultClient _vaultClient;

    public VaultFacade(IVaultClient vaultClient)
    {
        _vaultClient = vaultClient;
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadSecretAsync(string path, string mountPoint = "k8s")
    {
        _vaultClient.Settings.SecretsEngineMountPoints.KeyValueV2 = mountPoint;

        var kv2Secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<IDictionary<string, string?>>(path);

        if (kv2Secret?.Data?.Data is null)
        {
            throw new InvalidDataException($"Path: {path} of Mount Point: {mountPoint} returned null");
        }

        return kv2Secret.Data.Data.AsEnumerable();
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadRabbitMqSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.RabbitMQ);
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadCognitoSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.Cognito);
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadSESSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.SimpleEmailService);
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadSNSSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.SimpleNotificationService);
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadAgencyServiceConnectionStringSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.AgencyServiceDatabase);
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadUserServiceConnectionStringSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.UserServiceDatabase);
    }

    public async Task<IEnumerable<KeyValuePair<string, string?>>> ReadEmployeeServiceDatabaseSecretAsync()
    {
        return await ReadSecretAsync(VaultSecretsPaths.EmployeeServiceDatabase);
    }
}
