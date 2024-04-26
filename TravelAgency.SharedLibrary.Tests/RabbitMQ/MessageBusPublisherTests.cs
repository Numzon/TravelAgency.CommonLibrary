using AutoFixture;
using RabbitMQ.Client;
using TravelAgency.SharedLibrary.RabbitMQ;

namespace TravelAgency.SharedLibrary.Tests.RabbitMQ;
public sealed class MessageBusPublisherTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IConnection> _connection;
    private readonly Mock<IModel> _channel;
    private readonly Mock<IAsyncConnectionFactory> _factory;

    public MessageBusPublisherTests()
    {
        _fixture = new Fixture();
        _connection = new Mock<IConnection>();
        _factory = new Mock<IAsyncConnectionFactory>();
        _channel = new Mock<IModel>();

        _factory.Setup(x => x.CreateConnection()).Returns(_connection.Object);
        _connection.Setup(x => x.CreateModel()).Returns(_channel.Object);
        _channel.Setup(x => x.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()));
        _channel.Setup(x => x.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));
    }

    [Fact]
    public async Task Publish_ConnectionIsClosed_TaskCompleted()
    {
        _connection.Setup(x => x.IsOpen).Returns(false);

        var messageBusPublisher = new MessageBusPublisher(_factory.Object);

        await messageBusPublisher.Invoking(x => x.Publish(_fixture.Create<string>())).Should().NotThrowAsync();
    }

    [Fact]
    public async Task Publish_ConnectionIsOpen_TaskCompleted()
    {
        _connection.Setup(x => x.IsOpen).Returns(true);

        var messageBusPublisher = new MessageBusPublisher(_factory.Object);

        await messageBusPublisher.Invoking(x => x.Publish(_fixture.Create<string>())).Should().NotThrowAsync();
    }
}
