using Shared.Dependencies;
using Shared.Options;
using UserManagement.Api.DependencyInjection.Cors;
using UserManagement.Api.DependencyInjection.Mediator;
using UserManagement.Api.DependencyInjection.Options;

namespace UserManagement.Api.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .ConfigureCors()
            .AddMediator()
            .AddOptionsServiceCollection(builder.Configuration);
        
        var elkOptions = builder.Configuration.GetSection(ElkOptions.Elk).Get<ElkOptions>();
        builder.Host.ConfigureSerilog(elkOptions!.HttpSinkRequestUri,
            elkOptions.ServiceName
        );
    }
}