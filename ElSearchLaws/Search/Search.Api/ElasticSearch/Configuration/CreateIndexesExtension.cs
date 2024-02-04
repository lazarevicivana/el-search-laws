using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Analysis;
using Search.Api.Contracts;
using Search.Api.Laws;

namespace Search.Api.ElasticSearch.Configuration;

public static class CreateIndexesExtension
{
    public static void CreateLawIndex(this ElasticsearchClient client, string indexName)
    {
        client.Indices.Create<Law>(descriptor => descriptor
                /*.Settings(s => s
                    .Analysis(a => a
                        .Analyzers(aa=> aa
                            .Language("serbian", languageAnalyzer =>
                            {
                                languageAnalyzer.Language(Language.Spanish);
                            }))
                        )
                    )*/
                    /*.Mappings(mm=> mm
                        .AllField(af => af
                            .Analyzer("serbian")
                            .SearchAnalyzer("serbian")
                            .Store(true)))*/
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
                        .SearchAnalyzer("serbian"))
                    .Text(t => t.GovernmentName, c => c
                        .Analyzer("serbian")
                        .SearchAnalyzer("serbian"))
                    .Text(t => t.GovernmentType, c => c
                        .Analyzer("serbian")
                        .SearchAnalyzer("serbian"))
                ))
        );
    }
}