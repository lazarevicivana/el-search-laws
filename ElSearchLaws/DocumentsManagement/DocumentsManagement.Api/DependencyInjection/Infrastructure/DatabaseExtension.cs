using DocumentsManagement.Api.MinioConfig;
using Minio;

namespace DocumentsManagement.Api.DependencyInjection.Infrastructure;

public static class DatabaseExtension
{
    public static IServiceCollection ConfigureMinioDb(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMinio(x =>
        {
            var options = configuration
                .GetSection(MinioOptions.Minio)
                .Get<MinioOptions>();
            x.WithCredentials(options!.AccessKey, options.SecretKey);
            x.WithEndpoint(options.Endpoint);
            x.WithSSL(options.UseSsl);
        });
        return services;
    }
}