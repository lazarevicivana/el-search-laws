using System;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Search.Api.Contracts;
using Search.Api.ElasticSearch.Options;
using Search.Api.Laws;

namespace Search.Api.ElasticSearch.Configuration;

public static class ElasticSearchExtension
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services)
    {
        var elkOptions = services.BuildServiceProvider()
            .GetRequiredService<IOptions<ElasticSearchOptions>>().Value;

        var elkSettings = new ElasticsearchClientSettings(new Uri(elkOptions.Uri))
            .PrettyJson()
            .EnableDebugMode();
        
        var srbLawIndex = elkOptions.SrbLawIndex;
        var srbContractIndex = elkOptions.SrbContractIndex;
        elkSettings.AddAllMappings(srbLawIndex, srbContractIndex);
        
        var client = new ElasticsearchClient(elkSettings);
        client.CreateLawIndex(srbLawIndex);
        client.CreateContractIndex(srbContractIndex);
        services.AddSingleton(client);
        return services;
    }

    private static void AddAllMappings(this ElasticsearchClientSettings settings, string indexName1, string indexName2)
    {
        settings.AddLawMapping(indexName1);
        settings.AddContractMapping(indexName2);
    }

}