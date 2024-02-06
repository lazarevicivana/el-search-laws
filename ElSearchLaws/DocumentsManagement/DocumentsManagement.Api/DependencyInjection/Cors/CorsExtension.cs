namespace DocumentsManagement.Api.DependencyInjection.Cors;

public static class CorsExtension
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
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