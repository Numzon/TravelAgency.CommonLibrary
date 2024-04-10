using Amazon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TravelAgency.SharedLibrary.AWS;

namespace TravelAgency.SharedLibrary.Tests.AWS;
public sealed class SecretManagerConfigurationTests
{
    [Fact]
    public void AddAndConfigureSecretManager_AllNeededDataExists_ConfiguresSecretManager()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder();
        var environment = new Mock<IWebHostEnvironment>();
        var prefix = "dummy_prefix";

        environment.Setup(x => x.ApplicationName).Returns("APPLICATION.NAME");
        environment.Setup(x => x.EnvironmentName).Returns("ENVIRONMENT");

        builder.AddAndConfigureSecretManager(environment.Object, RegionEndpoint.APEast1, prefix);

        builder.Should().NotBeNull();
        builder.Sources.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public void AddAndConfigureSecretManager_GivenPrefixIsNull_ConfiguresSecretManager()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder();
        var environment = new Mock<IWebHostEnvironment>();
        string prefix = null!;

        environment.Setup(x => x.ApplicationName).Returns("APPLICATION.NAME");
        environment.Setup(x => x.EnvironmentName).Returns("ENVIRONMENT");

        builder.AddAndConfigureSecretManager(environment.Object, RegionEndpoint.EUCentral1, prefix);

        builder.Should().NotBeNull();
        builder.Sources.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public void AddAndConfigureSecretManager_RegionIsNull_ConfiguresSecretManager()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder();
        var environment = new Mock<IWebHostEnvironment>();
        RegionEndpoint region = null!;

        environment.Setup(x => x.ApplicationName).Returns("APPLICATION.NAME");
        environment.Setup(x => x.EnvironmentName).Returns("ENVIRONMENT");

        builder.Invoking(x => x.AddAndConfigureSecretManager(environment.Object, region))
            .Should().Throw<InvalidOperationException>().WithMessage("RegionEndpoint cannot be null");
    }
}
