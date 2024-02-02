using Elastic.Clients.Elasticsearch;
using Search.Api.Contracts;
using Search.Api.Laws;

namespace Search.Api.ElasticSearch.Configuration;

public static class MappingExtensions
{
    public static void AddLawMapping(this ElasticsearchClientSettings settings, string indexName)
    {
        settings.DefaultMappingFor<Law>(sm =>
            sm
                .IndexName(indexName)
                .IdProperty(x=> x.Id)
        );
    }
    public static void AddContractMapping(this ElasticsearchClientSettings settings, string indexName)
    {
        settings.DefaultMappingFor<Contract>(sm =>
            sm
                .IndexName(indexName)
                .IdProperty(x=> x.Id)
        );
    }
}