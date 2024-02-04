using DocumentsManagement.Api.MinioConfig;
using DocumentsManagement.Api.Options;
using Microsoft.Extensions.Options;
using Shared.Dependencies;

namespace DocumentsManagement.Api.DependencyInjection.OptionsExtensions;

public static class OptionsExtension
{
    public static IServiceCollection AddOptionsServiceCollection(this IServiceCollection services,IConfiguration configuration)
    {
        services.BindOptions(configuration);
        services.AddSingleton<IValidateOptions<MinioOptions>, MinioOptions>()
            .AddOptions<MinioOptions>()
            .Bind(configuration.GetSection(MinioOptions.Minio))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<PdfOptions>, PdfOptions>()
            .AddOptions<PdfOptions>()
            .Bind(configuration.GetSection(PdfOptions.Pdf))
            .ValidateOnStart();

        
        return services;
    }
}