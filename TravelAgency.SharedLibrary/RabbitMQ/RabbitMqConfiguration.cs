using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public static class RabbitMqConfiguration
{
    public static IServiceCollection AddRabbitMqConfiguration(this IServiceCollection services, RabbitMqSettingsDto settings)
    {
        var factory = new ConnectionFactory()
        {
            HostName = settings.Host,
            Port = Convert.ToInt32(settings.Port),
            UserName = settings.Username,
            Password = settings.Password
        };
        services.AddSingleton<IAsyncConnectionFactory>(factory);

        services.AddSingleton<IEventReceiver, EventReceiver>();

        services.AddSingleton<IMessageBusPublisher, MessageBusPublisher>();

        services.AddHostedService<MessageBusSubscriber>();

        return services;
    }
}
