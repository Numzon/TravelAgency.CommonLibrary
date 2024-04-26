using AutoFixture;
using RabbitMQ.Client;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.Tests.RabbitMQ;
public sealed class MessageBusSubscriberTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IEventReceiver> _eventReceiver;
    private readonly Mock<IConnection> _connection;
    private readonly Mock<IModel> _channel;
    private readonly Mock<IAsyncConnectionFactory> _factory;

    public MessageBusSubscriberTests()
    {
        _fixture = new Fixture();
        _eventReceiver = new Mock<IEventReceiver>();
        _connection = new Mock<IConnection>();
        _factory = new Mock<IAsyncConnectionFactory>();
        _channel = new Mock<IModel>();
        InitializeDefaultValuesForMocks();
    }

    [Fact]
    public void StartAsync_CancellationRequested_ThrowsOperationCanceledException()
    {
        var token = new CancellationToken(true);
        
        var messageBus = new MessageBusSubscriber(_factory.Object, _eventReceiver.Object);

        messageBus.Invoking(x => x.StartAsync(token)).Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void StartAsync_ConfiguredEventBasicConsumer_ConfiguredBackgroundService()
    {
        var token = new CancellationToken();

        var messageBus = new MessageBusSubscriber(_factory.Object, _eventReceiver.Object);

        messageBus.Invoking(x => x.StartAsync(token)).Should().NotThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StopAsync_StartedBackgroudService_SuccesfullyStopped()
    {
        var token = new CancellationToken();

        var messageBus = new MessageBusSubscriber(_factory.Object, _eventReceiver.Object);
        await messageBus.StartAsync(token);

        await messageBus.Invoking(x => x.StopAsync(token)).Should().NotThrowAsync<OperationCanceledException>();
    }

    private void InitializeDefaultValuesForMocks()
    {
        _factory.Setup(x => x.CreateConnection()).Returns(_connection.Object);
        _connection.Setup(x => x.CreateModel()).Returns(_channel.Object);
        _channel.Setup(x => x.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()));
        var queueDeclare = new QueueDeclareOk(_fixture.Create<string>(), _fixture.Create<uint>(), _fixture.Create<uint>());
        _channel.Setup(x => x.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>())).Returns(queueDeclare);
        _channel.Setup(x => x.QueueBind(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null));
    }
}
