using Elastic.Clients.Elasticsearch;
using Search.Api.Contracts;
using Search.Api.Laws;

namespace Search.Api.ElasticSearch.Configuration;

public static class CreateIndexesExtension
{
    public static void CreateLawIndex(this ElasticsearchClient client, string indexName)
    {
        client.Indices.Create<Law>(descriptor => descriptor
            .Mappings(mm => mm
                .Properties(pp=>pp
                    .Keyword(k=> k.Id)
                    .Text(t=>t.Content,tp=> tp
                        .Analyzer("serbian")
                    )
                    .Text(t=>t.Title,tp=> tp
                        .Analyzer("serbian")
                        .Fields(f=>f
                            .Keyword(k=> k.Title, v=> v
                                .IgnoreAbove(265)
                                .Suffix("keyword")
                            )
                        )
                    )
                    .Text(t=> t.FileName)
                ))
                .Index(indexName)
        );
    }
    public static void CreateContractIndex(this ElasticsearchClient client, string indexName)
    {
        client.Indices.Create<Contract>(descriptor => descriptor
            .Index(indexName)
            .Mappings(mm => mm
                .Properties(pp=> pp
                    .Keyword(t => t.Id )
                    .Text(t => t.Content, c => c
                        .Analyzer("serbian")
                        .SearchAnalyzer("serbian"))
                    .Date(t => t.CreatedAt)
                    .Text(t => t.SignatoryPersonName, c => c
                        .Analyzer("serbian")
                        .SearchAnalyzer("serbian"))
                    .Text(t => t.SignatoryPersonSurname, c => c
                        .Analyzer("serbian")
                        .Fields(f=>f
                            .Keyword(k=> k.SignatoryPersonSurname, v=> v
                                .IgnoreAbove(265)
                                .Suffix("keyword")))
                        .SearchAnalyzer("serbian"))
                    .Text(t => t.GovernmentName, c => c
                        .Analyzer("serbian")
                        .Fields(f=>f
                            .Keyword(k=> k.GovernmentName, v=> v
                                .IgnoreAbove(265)
                                .Suffix("keyword")))
                        .SearchAnalyzer("serbian"))
                    .Text(t => t.GovernmentType, c => c
                        .Analyzer("serbian")
                        .Fields(f=>f
                            .Keyword(k=> k.GovernmentType, v=> v
                                .IgnoreAbove(265)
                                .Suffix("keyword")))
                        .SearchAnalyzer("serbian"))
                ))
        );
    }
}