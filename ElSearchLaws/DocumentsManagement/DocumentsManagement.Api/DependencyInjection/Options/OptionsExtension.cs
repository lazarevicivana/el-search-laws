using Shared.Dependencies;

namespace DocumentsManagement.Api.DependencyInjection.Options;

public static class OptionsExtension
{
    public static IServiceCollection AddOptionsServiceCollection(this IServiceCollection services,IConfiguration configuration)
    {
        services.BindElkOptions(configuration);
        return services;
    }
}