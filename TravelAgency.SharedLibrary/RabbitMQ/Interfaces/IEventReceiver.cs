namespace TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
public interface IEventReceiver
{
    Task ProcessEvent(string message, CancellationToken cancellationToken);
}
