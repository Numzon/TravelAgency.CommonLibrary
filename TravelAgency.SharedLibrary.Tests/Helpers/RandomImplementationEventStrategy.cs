using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.Tests.Helpers;
internal sealed class RandomImplementationEventStrategy : IEventStrategy
{
    public Task ExecuteEvent(IServiceScope scope, string message)
    {
        return Task.CompletedTask;
    }
}
