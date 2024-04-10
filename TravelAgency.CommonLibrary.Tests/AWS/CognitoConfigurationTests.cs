using AutoFixture;
using Microsoft.IdentityModel.Tokens;
using TravelAgency.CommonLibrary.AWS;
using TravelAgency.CommonLibrary.Models;

namespace TravelAgency.CommonLibrary.Tests.AWS;
public sealed class CognitoConfigurationTests
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void AddAuthorizationWithPolicies_ShouldAddPolicies()
    {
        IServiceCollection service = new ServiceCollection();

        service.AddAuthorizationWithPolicies();

        service.Should().NotBeNull();
        service.FirstOrDefault(x => x.ServiceType.FullName is not null && x.ServiceType.FullName.StartsWith("Microsoft.AspNetCore.Authorization")).Should().NotBeNull();
    }

    [Fact]
    public void AddAuthenticationAndJwtConfiguration_AwsCognitoHasIncorrectAuthorityUrl_ThrowsException()
    {
        IServiceCollection service = new ServiceCollection();
        var settings = _fixture.Build<AwsCognitoSettingsDto>().Create();

        service.Invoking(x => x.AddAuthenticationAndJwtConfiguration(settings)).Should().Throw<Exception>();
    }
}
