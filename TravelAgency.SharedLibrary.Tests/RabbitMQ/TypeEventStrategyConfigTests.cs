using AutoFixture;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
using TravelAgency.SharedLibrary.Tests.Helpers;

namespace TravelAgency.SharedLibrary.Tests.RabbitMQ;
public sealed class TypeEventStrategyConfigTests
{
    private readonly Fixture _fixture;

    public TypeEventStrategyConfigTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void NewConfig_EventTypeStringIsNull_ThrowsArgumentNullException()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;
        string eventType = null!;

        config.Invoking(x => x.NewConfig<RandomImplementationEventStrategy>(eventType)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NewConfig_EventTypeStringIsEmpty_ThrowsArgumentNullException()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;
        string eventType = string.Empty;

        config.Invoking(x => x.NewConfig<RandomImplementationEventStrategy>(eventType)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NewConfig_EventTypeIsRandomString_DoesnThrowException()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;
        string eventType = _fixture.Create<string>();

        config.Invoking(x => x.NewConfig<RandomImplementationEventStrategy>(eventType)).Should().NotThrow();
    }

    [Fact]
    public void GetEventStrategy_EventTypeIsRandomString_ReturnsInstanceOfEventStrategy()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;
        string eventType = _fixture.Create<string>();

        config.NewConfig<RandomImplementationEventStrategy>(eventType);

        var result = config.GetEventStrategy(eventType);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo(typeof(IEventStrategy));
    }

    [Fact]
    public void GetEventStrategy_EventTypeIsNull_ThrowsArgumentNullException()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;

        config.Invoking(x => x.GetEventStrategy(null!)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetEventStrategy_EventTypeIsAnEmptyString_ThrowsArgumentNullException()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;

        config.Invoking(x => x.GetEventStrategy(null!)).Should().Throw<ArgumentNullException>();
    }


    [Fact]
    public void GetEventStrategy_InvalidEventType_ThrowsException()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;

        config.Invoking(x => x.GetEventStrategy(_fixture.Create<string>())).Should().Throw<InvalidOperationException>();
    }
}
