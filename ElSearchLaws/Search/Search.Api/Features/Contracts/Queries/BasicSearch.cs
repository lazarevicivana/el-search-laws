using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using FluentResults;
using MediatR;
using Search.Api.Contracts;
using Result = FluentResults.Result;

namespace Search.Api.Features.Contracts.Queries;

public static class BasicSearch
{
    public record Query(
        string Field,
        string Value) : IRequest<Result<ContractSearchResponse>>;
    
    internal class Handler(ElasticsearchClient client): IRequestHandler<Query,Result<ContractSearchResponse>>
    {
        public async Task<Result<ContractSearchResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var searchResponse = await client.SearchAsync<Contract>(s => s
                    .Query(q => q
                        .Match(m => m
                            .Field(request.Field)
                            .Analyzer("serbian")
                            .Query(request.Value)))
                    .Highlight(h => h
                        .Fields(fields => fields
                            .Add(request.Field, new HighlightField
                            {
                                Type = HighlighterType.Plain
                            }))
                    ), cancellationToken);
            if (!searchResponse.IsValidResponse)
                return Result.Fail("Something went wrong with searching contracts");
            var hits = searchResponse.Hits.ToList();
            var response = CreateSearchResponse(hits,request);
            return response;
        }

        private ContractSearchResponse CreateSearchResponse(List<Hit<Contract>> hits, Query request)
        {
            var hitResponses = new List<ContractHitResponse>();
            hits.ForEach(hit =>
            {
                if (hit.Highlight is not null && hit.Highlight.ContainsKey(request.Field))
                {
                    var highlightStrings = hit.Highlight[request.Field].ToList();
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

                if (hit.Highlight is null || !hit.Highlight.ContainsKey(request.Field))
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
    }
}