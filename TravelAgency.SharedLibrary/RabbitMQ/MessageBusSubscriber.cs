using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public sealed class MessageBusSubscriber : BackgroundService
{
    private readonly IEventReceiver _eventReceiver;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public MessageBusSubscriber(IAsyncConnectionFactory factory, IEventReceiver eventReceiver)
    {
        _eventReceiver = eventReceiver;
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(RabbitMqExchange.Trigger, ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(_queueName, RabbitMqExchange.Trigger, "");
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventReceiver.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(_queueName, true, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (_connection.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }

        base.Dispose();
    }
}
