using Microsoft.Extensions.DependencyInjection;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.SharedLibrary.RabbitMQ;
public static class RabbitMqConfiguration
{
    public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBusPublisher, MessageBusPublisher>();

        return services;
    } 

    public static IServiceCollection AddRabbitMqSubscriber(this IServiceCollection services)
    {
        services.AddSingleton<IEventReceiver, EventReceiver>();

        services.AddHostedService<MessageBusSubscriber>();

        return services;
    }
}
