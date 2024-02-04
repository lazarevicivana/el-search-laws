using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Search.Api.DependencyInjection.Infrastructure;
using Search.Api.DependencyInjection.Mediator;
using Search.Api.DependencyInjection.Options;
using Search.Api.ElasticSearch.Configuration;
using Shared.Dependencies;
using Shared.Options;

namespace Search.Api.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddCors()
            .AddMediator()
            .AddOptionsServiceCollection(builder.Configuration)
            .AddElasticSearch()
            .AddInfrastructure();
        
        var elkOptions = builder.Configuration.GetSection(ElkOptions.Elk).Get<ElkOptions>();
        builder.Host.ConfigureSerilog(elkOptions!.HttpSinkRequestUri,
            elkOptions.ServiceName
        );
    }
}