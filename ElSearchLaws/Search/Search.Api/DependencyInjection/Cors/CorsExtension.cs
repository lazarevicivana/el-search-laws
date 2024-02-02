using Microsoft.Extensions.DependencyInjection;

namespace Search.Api.DependencyInjection.Cors;

public static class CorsExtension
{
    public static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options => {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
        });
        return services;
    }
}