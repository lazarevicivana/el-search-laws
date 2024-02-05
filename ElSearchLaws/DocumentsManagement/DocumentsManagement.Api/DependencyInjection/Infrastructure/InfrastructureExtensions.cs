using DocumentsManagement.Api.Features.Common;
using DocumentsManagement.Api.Features.Contracts;
using DocumentsManagement.Api.Features.Contracts.ExctractContractContent;
using DocumentsManagement.Api.Features.Laws;
using DocumentsManagement.Api.Features.Laws.ExtractLawContents;
using DocumentsManagement.Api.Options;

namespace DocumentsManagement.Api.DependencyInjection.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        AddIronPdfLicence(configuration);
        services.AddMessageBroker();
        services.AddScoped<IContentExtractor<Law>, ExtractorLawContent>();
        services.AddScoped<IContentExtractor<Contract>, ExtractContractContent>();
        services.ConfigureMinioDb(configuration);
        
        return services;
    }
    private static void AddIronPdfLicence(IConfiguration configuration)
    {
        var options = configuration
            .GetSection(PdfOptions.Pdf)
            .Get<PdfOptions>()!;

        License.LicenseKey = options.License;
    }
}