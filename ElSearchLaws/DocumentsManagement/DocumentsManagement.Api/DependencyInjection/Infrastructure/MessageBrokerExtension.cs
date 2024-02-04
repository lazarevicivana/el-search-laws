using MassTransit;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace DocumentsManagement.Api.DependencyInjection.Infrastructure;

public static class MessageBrokerExtension
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();
            config.UsingRabbitMq((context, rabbitMqConfig) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                rabbitMqConfig.Host(rabbitMqOptions.Host, hostConfig =>
                {
                    hostConfig.Username(rabbitMqOptions.Username);
                    hostConfig.Password(rabbitMqOptions.Password);
                });
                rabbitMqConfig.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}