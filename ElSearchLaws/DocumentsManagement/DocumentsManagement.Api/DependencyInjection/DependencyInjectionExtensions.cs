using DocumentsManagement.Api.DependencyInjection.Mediator;
using DocumentsManagement.Api.DependencyInjection.Options;
using Shared.Dependencies;
using Shared.Options;

namespace DocumentsManagement.Api.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCors()
            .AddMediator()
            .AddOptionsServiceCollection(builder.Configuration);
        
        var elkOptions = builder.Configuration.GetSection(ElkOptions.Elk).Get<ElkOptions>();
        builder.Host.ConfigureSerilog(elkOptions!.HttpSinkRequestUri,
            elkOptions.ServiceName
        );
    }
}