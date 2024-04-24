using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public sealed class MessageBusPublisher : IMessageBusPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusPublisher(IOptions<RabbitMqSettingsDto> options)
    {
        var settings = options.Value;
        var factory = new ConnectionFactory()
        {
            HostName = settings.Host,
            Port = Convert.ToInt32(settings.Port),
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(RabbitMqExchange.Trigger, ExchangeType.Fanout);
    }

    public Task Publish(string message)
    {
        if (_connection.IsOpen)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: RabbitMqExchange.Trigger, routingKey: "", basicProperties: null, body: body);
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_connection.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
