using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public sealed class EventReceiver : IEventReceiver
{
    private readonly IServiceScopeFactory _factory;
    private readonly TypeEventStrategyConfig _config;

    public EventReceiver(IServiceScopeFactory factory, TypeEventStrategyConfig? config = null)
    {
        _factory = factory;
        _config = config ?? TypeEventStrategyConfig.GlobalSetting;
    }

    public async Task ProcessEvent(string message)
    {
        var eventStrategy = DecodeEventAndReturnEventStrategy(message);

        using (var scope = _factory.CreateScope())
        {
            await eventStrategy.ExecuteEvent(scope, message);
        }
    }

    private IEventStrategy DecodeEventAndReturnEventStrategy(string notificationMessage)
    {
        var jObj = JObject.Parse(notificationMessage);

        if (jObj.TryGetValue(nameof(BasePublishedDto.Event), out var eventType) is false)
        {
            throw new InvalidOperationException("Given json does not contains event property");
        }

        var strategy = _config.GetEventStrategy(eventType.ToString());

        return strategy;
    }
}