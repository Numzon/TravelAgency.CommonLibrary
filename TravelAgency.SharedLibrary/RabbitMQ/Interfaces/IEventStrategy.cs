using Microsoft.Extensions.DependencyInjection;

namespace TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
public interface IEventStrategy
{
    Task ExecuteEvent(IServiceScope scope, string message);
}
