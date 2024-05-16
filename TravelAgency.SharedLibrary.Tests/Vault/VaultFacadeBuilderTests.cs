using AutoFixture;
using Microsoft.Extensions.Hosting;
using TravelAgency.SharedLibrary.Vault;

namespace TravelAgency.SharedLibrary.Tests.Vault;
public sealed class VaultFacadeBuilderTests
{
    private readonly Fixture _fixture;

    public VaultFacadeBuilderTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void SetToken_GivenStringIsANull_ThrowsArgumentNullException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string token = null!;

        vaultFacadeBuilder.Invoking(x => x.SetToken(token)).Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void SetPort_GivenStringIsANull_ThrowsArgumentNullException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string port = null!;

        vaultFacadeBuilder.Invoking(x => x.SetPort(port)).Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void SetPort_GivenStringCannotBeConvertedToInt_ThrowsArgumentNullException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string port = _fixture.Create<string>();

        vaultFacadeBuilder.Invoking(x => x.SetPort(port)).Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void SetPort_GivenStringIsZero_ThrowsArgumentNullException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string port = "0";

        vaultFacadeBuilder.Invoking(x => x.SetPort(port)).Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void SetPort_GivenIntIsZero_ThrowsArgumentNullException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        int port = 0;

        vaultFacadeBuilder.Invoking(x => x.SetPort(port)).Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void SetHost_GivenStringIsANull_ThrowsArgumentNullException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string host = null!;

        vaultFacadeBuilder.Invoking(x => x.SetHost(host)).Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void SetHost_ValidBooleanValue_NotThrow()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        bool ssl = true;

        vaultFacadeBuilder.Invoking(x => x.SetSSL(ssl)).Should().NotThrow();
    }

    [Fact]
    public void SetToken_ValidString_NotThrow()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string token = _fixture.Create<string>();

        vaultFacadeBuilder.Invoking(x => x.SetToken(token)).Should().NotThrow();
    }

    [Fact]
    public void SetPort_ValidIntAsString_NotThrow()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string port = _fixture.Create<int>().ToString();

        vaultFacadeBuilder.Invoking(x => x.SetPort(port)).Should().NotThrow();
    }

    [Fact]
    public void SetHost_ValidString_NotThrow()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        string host = _fixture.Create<string>();

        vaultFacadeBuilder.Invoking(x => x.SetHost(host)).Should().NotThrow();
    }

    [Fact]
    public void Build_PortNotSet_ThrowsInvalidOperationException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        vaultFacadeBuilder
            .SetToken(_fixture.Create<string>())
            .SetHost(_fixture.Create<string>());

        vaultFacadeBuilder.Invoking(x => x.Build()).Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Build_TokenNotSet_ThrowsInvalidOperationException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        vaultFacadeBuilder
            .SetPort(_fixture.Create<int>().ToString())
            .SetHost(_fixture.Create<string>());

        vaultFacadeBuilder.Invoking(x => x.Build()).Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Build_HostNotSet_ThrowsInvalidOperationException()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        vaultFacadeBuilder
            .SetToken(_fixture.Create<string>())
            .SetPort(_fixture.Create<int>().ToString());

        vaultFacadeBuilder.Invoking(x => x.Build()).Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Build_AllNecesseryParametersSet_ReturnVaultFacade()
    {
        var vaultFacadeBuilder = new VaultFacadeBuilder();
        vaultFacadeBuilder
            .SetToken(_fixture.Create<string>())
            .SetPort(_fixture.Create<int>().ToString())
            .SetHost(_fixture.Create<int>().ToString());

        var facade = vaultFacadeBuilder.Build();

        facade.Should().NotBeNull();    
    }
}
