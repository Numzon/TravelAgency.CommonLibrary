using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public sealed class TypeEventStrategyConfig
{
    public static TypeEventStrategyConfig GlobalSetting => new TypeEventStrategyConfig();

    private readonly IDictionary<string, Type> _eventStrategies;

    public TypeEventStrategyConfig()
    {
        _eventStrategies = new Dictionary<string, Type>();
    }

    public void NewConfig<TEventStrategy>(string eventType)
        where TEventStrategy : IEventStrategy, new()
    {
        _eventStrategies.Add(eventType, typeof(TEventStrategy));
    }

    public IEventStrategy GetEventStrategy(string eventType)
    {
        if (_eventStrategies.TryGetValue(eventType, out var type) is false)
        {
            throw new InvalidOperationException($"EventStrategy for given event type ({eventType}), hasn't been defined yet");
        }

        var strategy = Activator.CreateInstance(type);

        if (strategy is null)
        {
            throw new InvalidOperationException($"Type {nameof(type)} cannot be created");
        }
        return (IEventStrategy)strategy;
    }
}