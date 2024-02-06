using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NiceToGift.BuildingBlocks.Infrastructure.HttpHandler;
using NiceToGift.Shops.Infrastructure.LocationIq;
using Search.Api.DependencyInjection.Cors;
using Search.Api.DependencyInjection.Infrastructure;
using Search.Api.DependencyInjection.Mediator;
using Search.Api.DependencyInjection.Options;
using Search.Api.ElasticSearch.Configuration;
using Search.Api.LocationIq;
using Shared.Dependencies;
using Shared.Options;
using HttpClientHandler = Search.Api.HttpHandler.HttpClientHandler;

namespace Search.Api.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .ConfigureCors()
            .AddMediator()
            .AddOptionsServiceCollection(builder.Configuration)
            .AddElasticSearch()
            .Configure<LocationIqConfig>(builder.Configuration.GetSection(LocationIqConfig.SectionName))
            .AddScoped<ILocationClient,LocationIqClient>()
            .AddScoped<IHttpClientHandler,HttpClientHandler>()
            .AddInfrastructure();
        
        var elkOptions = builder.Configuration.GetSection(ElkOptions.Elk).Get<ElkOptions>();
        builder.Host.ConfigureSerilog(elkOptions!.HttpSinkRequestUri,
            elkOptions.ServiceName
        );
    }
}