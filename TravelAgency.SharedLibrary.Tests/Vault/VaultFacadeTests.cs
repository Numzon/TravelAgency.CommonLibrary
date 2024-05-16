using Amazon.Runtime.Internal.Transform;
using AutoFixture;
using TravelAgency.SharedLibrary.Vault;
using VaultSharp;
using VaultSharp.V1;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SecretsEngines.KeyValue;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;

namespace TravelAgency.SharedLibrary.Tests.Vault;
public sealed class VaultFacadeTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IVaultClient> _vaultClient;
    private readonly Mock<IKeyValueSecretsEngineV2> _keyValueV2;

    public VaultFacadeTests()
    {
        _fixture = new Fixture();
        _vaultClient = new Mock<IVaultClient>();
        _keyValueV2 = new Mock<IKeyValueSecretsEngineV2>();

        var vaultClientV1 = new Mock<IVaultClientV1>();
        var secretsEngine = new Mock<ISecretsEngine>();
        var keyValue = new Mock<IKeyValueSecretsEngine>();

        keyValue.Setup(x => x.V2).Returns(_keyValueV2.Object);
        secretsEngine.Setup(x => x.KeyValue).Returns(keyValue.Object);
        vaultClientV1.Setup(x => x.Secrets).Returns(secretsEngine.Object);
        _vaultClient.Setup(x => x.V1).Returns(vaultClientV1.Object);

        var auth = new TokenAuthMethodInfo(_fixture.Create<string>());
        var vaultClientSettings = new VaultClientSettings($"https://{_fixture.Create<string>()}:{_fixture.Create<int>()}", auth);

        _vaultClient.Setup(x => x.Settings).Returns(vaultClientSettings);
    }

    [Fact]
    public async Task ReadSecretAsync_ContainsOneValidSecret_ReturnsSecretAsEnumerable()
    {
        var secret = new Secret<SecretData<IDictionary<string, string?>>>();
        secret.Data = new SecretData<IDictionary<string, string?>>();
        secret.Data.Data = new Dictionary<string, string?>()
        {
            new KeyValuePair<string, string?>(_fixture.Create<string>(), _fixture.Create<string>())
        };
        _keyValueV2.Setup(x => x.ReadSecretAsync<IDictionary<string, string?>>(It.IsAny<string>(), null!, null!, null!)).ReturnsAsync(secret);

        var vault = new VaultFacade(_vaultClient.Object);

        var result = await vault.ReadSecretAsync(_fixture.Create<string>(), _fixture.Create<string>());

        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task ReadSecretAsync_ReturnsNullAsSecretEngineDoesnExist_ThrowsInvalidDataException()
    {
        var secret = new Secret<SecretData<IDictionary<string, string?>>>();
        secret.Data = new SecretData<IDictionary<string, string?>>();
        _keyValueV2.Setup(x => x.ReadSecretAsync<IDictionary<string, string?>>(It.IsAny<string>(), null!, null!, null!)).ReturnsAsync(secret);

        var vault = new VaultFacade(_vaultClient.Object);

        await vault.Invoking(x => x.ReadSecretAsync(_fixture.Create<string>(), _fixture.Create<string>())).Should().ThrowExactlyAsync<InvalidDataException>();
    }

    [Fact]
    public async Task ReadSecretAsync_SecretsEngineDoesNotHaveSecrets_ReturnsSecretAsEnumerable()
    {
        var secret = new Secret<SecretData<IDictionary<string, string?>>>();
        secret.Data = new SecretData<IDictionary<string, string?>>();
        secret.Data.Data = new Dictionary<string, string?>();
        _keyValueV2.Setup(x => x.ReadSecretAsync<IDictionary<string, string?>>(It.IsAny<string>(), null!, null!, null!)).ReturnsAsync(secret);

        var vault = new VaultFacade(_vaultClient.Object);

        var result = await vault.ReadSecretAsync(_fixture.Create<string>(), _fixture.Create<string>());

        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }
}
