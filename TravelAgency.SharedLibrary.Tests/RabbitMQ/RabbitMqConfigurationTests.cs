using AutoFixture;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.Tests.RabbitMQ;
public sealed class RabbitMqConfigurationTests
{
    private readonly Fixture _fixture;

    public RabbitMqConfigurationTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AddRabbitMqConfiguration_ShouldAddPublisher()
    {
        IServiceCollection service = new ServiceCollection();
        var settings = _fixture.Build<RabbitMqSettingsDto>().With(x => x.Port, _fixture.Create<int>().ToString()).Create();

        service.AddRabbitMqConfiguration(settings);


        service.Should().NotBeNull();
        service.FirstOrDefault(x => x.ServiceType.FullName is not null && x.ServiceType.FullName.Contains(nameof(IMessageBusPublisher))).Should().NotBeNull();
    }

    [Fact]
    public void AddRabbitMqConfiguration_ShouldAddEventReceiver()
    {
        IServiceCollection service = new ServiceCollection();
        var settings = _fixture.Build<RabbitMqSettingsDto>().With(x => x.Port, _fixture.Create<int>().ToString()).Create();

        service.AddRabbitMqConfiguration(settings);

        service.Should().NotBeNull();
        service.FirstOrDefault(x => x.ServiceType.FullName is not null && x.ServiceType.FullName.Contains(nameof(IEventReceiver))).Should().NotBeNull();
    }

    [Fact]
    public void AddRabbitMqConfiguration_ShouldAddMessageBusSubscriber()
    {
        IServiceCollection service = new ServiceCollection();
        var settings = _fixture.Build<RabbitMqSettingsDto>().With(x => x.Port, _fixture.Create<int>().ToString()).Create();

        service.AddRabbitMqConfiguration(settings);

        service.Should().NotBeNull();
        service.FirstOrDefault(x => x.ServiceType.FullName is not null && x.ServiceType.FullName.Contains(nameof(IHostedService))).Should().NotBeNull();
        service.FirstOrDefault(x => x.ImplementationType is not null && x.ImplementationType.Name.Equals(nameof(MessageBusSubscriber))).Should().NotBeNull();
    }

    [Fact]
    public void AddRabbitMqConfiguration_ShouldAddIAsyncConnectionFactory()
    {
        IServiceCollection service = new ServiceCollection();
        var settings = _fixture.Build<RabbitMqSettingsDto>().With(x => x.Port, _fixture.Create<int>().ToString()).Create();

        service.AddRabbitMqConfiguration(settings);

        service.Should().NotBeNull();
        service.FirstOrDefault(x => x.ServiceType.FullName is not null && x.ServiceType.FullName.Contains(nameof(IAsyncConnectionFactory))).Should().NotBeNull();
    }
}
