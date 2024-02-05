using MassTransit;
using Microsoft.Extensions.Options;
using Search.Api.MessageBroker;
using Shared.Options;

namespace Search.Api.DependencyInjection.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            AddConsumers(config);
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
    private static void AddConsumers(IRegistrationConfigurator configurator)
    {
        configurator.AddConsumer(typeof(UploadedDocumentConsumer));
        configurator.AddConsumer(typeof(ExportedLawMetadataConsumer));
        configurator.AddConsumer(typeof(ExportedContractMetadataConsumer));
    }
}