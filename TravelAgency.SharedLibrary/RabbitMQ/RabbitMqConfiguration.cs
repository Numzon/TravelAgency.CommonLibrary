using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public static class RabbitMqConfiguration
{
    public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBusPublisher, MessageBusPublisher>();

        return services;
    }

    public static IServiceCollection AddRabbitMqSubscriber(this IServiceCollection services, RabbitMqSettingsDto settings)
    {
        services.AddSingleton<IEventReceiver, EventReceiver>();

        var factory = new ConnectionFactory()
        {
            HostName = settings.Host,
            Port = Convert.ToInt32(settings.Port)
        };

        services.AddSingleton<IAsyncConnectionFactory>(factory);

        services.AddHostedService<MessageBusSubscriber>();

        return services;
    }
}
