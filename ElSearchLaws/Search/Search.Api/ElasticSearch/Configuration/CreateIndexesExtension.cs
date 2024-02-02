using Elastic.Clients.Elasticsearch;
using Search.Api.Contracts;
using Search.Api.Laws;

namespace Search.Api.ElasticSearch.Configuration;

public static class CreateIndexesExtension
{
    public static void CreateLawIndex(this ElasticsearchClient client, string indexName)
    {
        client.Indices.Create<Law>(descriptor => descriptor
            .Index(indexName)
        );
    }
    public static void CreateContractIndex(this ElasticsearchClient client, string indexName)
    {
        client.Indices.Create<Contract>(descriptor => descriptor
            .Index(indexName)
        );
    }
}