using TravelAgency.SharedLibrary.Swagger;

namespace TravelAgency.SharedLibrary.Tests.Swagger;
public sealed class SwaggerConfigurationTests
{
    [Fact]
    public void AddAndConfigureSwagger_RandomServiceName_AddsSwaggerToServiceCollection()
    {
        IServiceCollection service = new ServiceCollection();
        var serviceName = "dummy_service";

        service.AddAndConfigureSwagger(serviceName);

        service.Should().NotBeNull();
        service.Should().HaveCountGreaterThan(0);
        service.FirstOrDefault(x => x.ServiceType.Name.StartsWith("Swagger")).Should().NotBeNull();     
    }

    [Fact]
    public void AddAndConfigureSwagger_NullAsAParameter_AddsSwaggerToServiceCollection()
    {
        IServiceCollection service = new ServiceCollection();
        string serviceName = null!;

        service.AddAndConfigureSwagger(serviceName);

        service.Should().NotBeNull();
        service.Should().HaveCountGreaterThan(0);
        service.FirstOrDefault(x => x.ServiceType.Name.StartsWith("Swagger")).Should().NotBeNull();
    }
}
