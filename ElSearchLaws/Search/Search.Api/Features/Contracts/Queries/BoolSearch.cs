using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentResults;
using MediatR;
using Search.Api.Contracts;
using Search.Api.Features.Contracts.Dto;
using Result = FluentResults.Result;

namespace Search.Api.Features.Contracts.Queries;

public static class BoolSearch
{
    public record Query(string BoolQuery) : IRequest<Result<ContractSearchResponse>>;
    
    internal class Handler(ElasticsearchClient client):IRequestHandler<Query,Result<ContractSearchResponse>>
    {
        public async Task<Result<ContractSearchResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var parsedQuery = ParseQuery(request.BoolQuery);
            var condition = parsedQuery.GetCondition();
            var query = BuildQuery(condition);
            var searchResponses = await client.SearchAsync<Contract>(s => s
                .Query(query)
                .Highlight(h => h
                    .Fields(f =>f
                        .Add("content",new HighlightField
                        {
                            Type = HighlighterType.Plain
                        })
                        )), 
               cancellationToken);
            if (!searchResponses.IsValidResponse)
                return Result.Fail("Something wen wrong with bool search!");
            var hits = searchResponses.Hits.ToList();
            var result = CreateSearchResponse(hits);
            return result;
        }

        private ContractSearchResponse CreateSearchResponse(List<Hit<Contract>> hits)
        {
            var hitResponses = new List<ContractHitResponse>();
            hits.ForEach(hit =>
            {
                if (hit.Highlight is not null && hit.Highlight.ContainsKey("content"))
                {
                    var highlightStrings = hit.Highlight["content"].ToList();
                    var highlight = "..." + string.Join(" ... ", highlightStrings) + "...";
                    var hitResponse = new ContractHitResponse(hit.Source!.GovernmentName,
                        hit.Source.GovernmentType,
                        hit.Source.SignatoryPersonName,
                        hit.Source.SignatoryPersonSurname,
                        highlight,
                        hit.Source.Content,
                        hit.Source.FileName,
                        hit.Source.Id
                    );
                    hitResponses.Add(hitResponse);
                }

                if (hit.Highlight is null || !hit.Highlight.ContainsKey("content"))
                {
                    var highlight = hit.Source!.Content.Length > 150 
                        ? hit.Source.Content[..150] + "..."
                        : hit.Source.Content;
                    var hitResponse = new ContractHitResponse(hit.Source!.GovernmentName,
                        hit.Source.GovernmentType,
                        hit.Source.SignatoryPersonName,
                        hit.Source.SignatoryPersonSurname,
                        highlight,
                        hit.Source.Content,
                        hit.Source.FileName,
                        hit.Source.Id
                    );
                    hitResponses.Add(hitResponse);
                }
            });
            return new ContractSearchResponse(hitResponses, hits.Count);
        }

        private SearchQuery BuildQuery(SearchCondition condition)
        {
            if (condition is BasicSearchDto simpleCondition)
            {
                return simpleCondition.IsPhrase
                    ? new MatchPhraseQuery(Field.KeyField) {
                        Query = simpleCondition.Value, 
                        Field = simpleCondition.Field,
                        Analyzer = "serbian"
                    }
                    : new MatchQuery(Field.KeyField)
                    {
                        Query = simpleCondition.Value,
                        Field = simpleCondition.Field,
                        Analyzer = "serbian"
                    };
            }
            if (condition is BoolQueryDto booleanCondition)
            {
                var innerQueries = new List<Elastic.Clients.Elasticsearch.QueryDsl.Query>();
                booleanCondition.BoolQueryFields.ForEach(innerQuery =>
                {
                    innerQueries.Add(BuildQuery(innerQuery.GetCondition())!);
                });
                switch (booleanCondition.Operator)
                {
                    case " AND ":
                        return new BoolQuery
                        {
                            Must = innerQueries,
                        };
                    case " OR ":
                        return new BoolQuery
                        {
                            Should = innerQueries,
                            MinimumShouldMatch = 1
                        };
                    case " NOT ":
                        return new BoolQuery
                        {
                            MustNot = innerQueries
                        };
                }
            }
            return new MatchQuery(Field.KeyField)
            {
                Query = "",
                Field = "",
                Analyzer = "serbian"
            };
        }

        private SearchConditionWrapper ParseQuery(string request)
        {
            if (request.Contains(" OR "))
                return ParseBoolQuery(" OR ", request.Split(" OR "));
            if (request.Contains(" AND "))
                return ParseBoolQuery(" AND ", request.Split(" AND "));
            if (request.Contains("NOT "))
                return ParseNotQuery(request.Substring(4));
            return ParseBasicSearch(request);

        }

        private SearchConditionWrapper ParseNotQuery( string query)
        {
            var boolQueryDto = new BoolQueryDto
            {
                Operator = " NOT "
            };
            var conditions = new List<SearchConditionWrapper>();
            conditions.Add(ParseQuery(query));
            boolQueryDto.BoolQueryFields = conditions;
            return new SearchConditionWrapper
            {
                BoolQueryDto = boolQueryDto
            };
        }

        private SearchConditionWrapper ParseBasicSearch(string request)
        {
            var parts = request.Split(":");
            var basicSearchDto = new BasicSearchDto
            {
                Field = parts[0],
                Value = parts[1],
                IsPhrase = false
            };
            if (basicSearchDto.Value.StartsWith("\"") && basicSearchDto.Value.EndsWith("\""))
            {
                basicSearchDto.IsPhrase = true;
                basicSearchDto.Value = basicSearchDto.Value.Substring(1, basicSearchDto.Value.Length - 1);
            }
            return new SearchConditionWrapper
            {
                BasicSearchDto = basicSearchDto
            };
        }

        private SearchConditionWrapper ParseBoolQuery(string @operator, string[] parts)
        {
            var boolQueryDto = new BoolQueryDto
            {
                Operator = @operator,
            };
            var conditions = new List<SearchConditionWrapper>();
            
            foreach (var part in parts)
            {
                conditions.Add(ParseQuery(part));
            }

            boolQueryDto.BoolQueryFields = conditions;
            return new SearchConditionWrapper
            {
                BoolQueryDto = boolQueryDto
            };
        }
    }
}