using AutoFixture;
using TravelAgency.SharedLibrary.Swagger;
using TravelAgency.SharedLibrary.Vault;
using TravelAgency.SharedLibrary.Vault.Consts;

namespace TravelAgency.SharedLibrary.Tests.Vault;
public sealed class VaultConfigurationTests
{
    private readonly Fixture _fixture;

    public VaultConfigurationTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AddAndConfigureSwagger_RandomServiceName_ThrowsArgumentException()
    {
        IServiceCollection service = new ServiceCollection();

        service.Invoking(x => x.ConfigureVault()).Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AddAndConfigureSwagger_DefinedEnvironments_AddsIVaultClientToServiceCollection()
    {
        IServiceCollection service = new ServiceCollection();
        Environment.SetEnvironmentVariable(VaultEnvironmentVariables.Token, _fixture.Create<string>());
        Environment.SetEnvironmentVariable(VaultEnvironmentVariables.Port, _fixture.Create<int>().ToString());
        Environment.SetEnvironmentVariable(VaultEnvironmentVariables.Host, _fixture.Create<int>().ToString());

        service.ConfigureVault();

        service.Should().NotBeNull();
        service.Should().HaveCountGreaterThan(0);
        service.FirstOrDefault(x => x.ServiceType.Name.StartsWith("IVaultClient")).Should().NotBeNull();
    }

    [Fact]
    public void AddAndConfigureSwagger_DefinedEnvironmentsSSLAsTrue_AddsSwaggerToServiceCollection()
    {
        IServiceCollection service = new ServiceCollection();
        Environment.SetEnvironmentVariable(VaultEnvironmentVariables.Token, _fixture.Create<string>());
        Environment.SetEnvironmentVariable(VaultEnvironmentVariables.Port, _fixture.Create<int>().ToString());
        Environment.SetEnvironmentVariable(VaultEnvironmentVariables.Host, _fixture.Create<int>().ToString());

        service.ConfigureVault(true);

        service.Should().NotBeNull();
        service.Should().HaveCountGreaterThan(0);
        service.FirstOrDefault(x => x.ServiceType.Name.StartsWith("IVaultClient")).Should().NotBeNull();
    }
}
