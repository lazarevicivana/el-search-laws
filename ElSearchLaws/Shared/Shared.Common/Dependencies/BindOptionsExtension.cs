using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Shared.Dependencies;

public static class BindOptionsExtension
{
    public static IServiceCollection BindOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<ElkOptions>, ElkOptions>()
            .AddOptions<ElkOptions>()
            .Bind(configuration.GetSection(ElkOptions.Elk))
            .ValidateOnStart();
        
        services.AddSingleton<IValidateOptions<RabbitMqOptions>, RabbitMqOptions>()
            .AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptions.RabbitMq))
            .ValidateOnStart();
        
        return services;
    }
    
}