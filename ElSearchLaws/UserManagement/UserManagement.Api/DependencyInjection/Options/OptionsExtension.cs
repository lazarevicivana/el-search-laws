using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dependencies;

namespace UserManagement.Api.DependencyInjection.Options;

public static class OptionsExtension
{
    public static IServiceCollection AddOptionsServiceCollection(this IServiceCollection services,IConfiguration configuration)
    {
        services.BindOptions(configuration);
        
        return services;
    }
}