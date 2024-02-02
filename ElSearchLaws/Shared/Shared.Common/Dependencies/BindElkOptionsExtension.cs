using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Shared.Dependencies;

public static class BindElkOptionsExtension
{
    public static IServiceCollection BindElkOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<ElkOptions>, ElkOptions>()
            .AddOptions<ElkOptions>()
            .Bind(configuration.GetSection(ElkOptions.Elk))
            .ValidateOnStart();
        return services;
    }
    
}