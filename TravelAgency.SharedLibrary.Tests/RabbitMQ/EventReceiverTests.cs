using AutoFixture;
using System.Text.Json;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.SharedLibrary.Tests.Helpers;

namespace TravelAgency.SharedLibrary.Tests.RabbitMQ;
public sealed class EventReceiverTests
{
	private readonly Fixture _fixture;
	private readonly Mock<IServiceScopeFactory> _factory;
	private readonly TypeEventStrategyConfig _config;
	private readonly string _eventTypeName;

    public EventReceiverTests()
	{
		_fixture = new Fixture();
		_factory = new Mock<IServiceScopeFactory>();
		_config = TypeEventStrategyConfig.GlobalSetting;
		_eventTypeName = _fixture.Create<string>();

		_factory.Setup(x => x.CreateScope()).Returns(Mock.Of<IServiceScope>());
    }

	[Fact]
	public void ProcessEvent_MessageCannotBeParsed_ThrowsInvalidOperationException()
	{
        _config.NewConfig<RandomImplementationEventStrategy>(_eventTypeName);

        var eventReceiver = new EventReceiver(_factory.Object, _config);

		eventReceiver.Invoking(x => x.ProcessEvent(_fixture.Create<string>())).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public void ProcessEvent_RandomClassThatInheritedFromBaseClassWithIncorrectEvent_ThrowsInvalidOperationException()
    {
        _config.NewConfig<RandomImplementationEventStrategy>(_eventTypeName);
		var randomPublished = _fixture.Create<RandomPublishedDto>();
		

        var eventReceiver = new EventReceiver(_factory.Object, _config);

        eventReceiver.Invoking(x => x.ProcessEvent(JsonSerializer.Serialize(randomPublished))).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public void ProcessEvent_RandomClassThatInheritedFromBaseClassWithIncorrectEvent_ExecutedEvent()
    {
        _config.NewConfig<RandomImplementationEventStrategy>(_eventTypeName);
        var randomPublished = _fixture.Build<RandomPublishedDto>().With(x => x.Event, _eventTypeName).Create();


        var eventReceiver = new EventReceiver(_factory.Object, _config);

        eventReceiver.Invoking(x => x.ProcessEvent(JsonSerializer.Serialize(randomPublished))).Should().NotThrowAsync<InvalidOperationException>();
    }
}
