namespace TravelAgency.SharedLibrary.RabbitMQ.Interfaces;
public interface IMessageBusPublisher
{
    Task Publish(string message);
}
