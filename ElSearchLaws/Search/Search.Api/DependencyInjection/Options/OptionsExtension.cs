using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Search.Api.ElasticSearch.Options;
using Shared.Dependencies;

namespace Search.Api.DependencyInjection.Options;

public static class OptionsExtension
{
    public static IServiceCollection AddOptionsServiceCollection(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<ElasticSearchOptions>, ElasticSearchOptions>()
            .AddOptions<ElasticSearchOptions>()
            .Bind(configuration.GetSection(ElasticSearchOptions.ElasticSearch))
            .ValidateOnStart();
        
        services.BindElkOptions(configuration);
        return services;
    }
}