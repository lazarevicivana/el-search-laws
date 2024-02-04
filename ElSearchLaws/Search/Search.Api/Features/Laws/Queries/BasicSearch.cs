using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using FluentResults;
using MediatR;
using Search.Api.Laws;
using Result = FluentResults.Result;

namespace Search.Api.Features.Laws.Queries;

public static class BasicSearch
{
    public record Query(string Content) : IRequest<Result<SearchResponse>>;

    public record SearchResponse(List<HitResponse> Hits);

    public record HitResponse(
        string Content,
        string FileName);
    
    internal class Handler(ElasticsearchClient client): IRequestHandler<Query,Result<SearchResponse>>
    {
        public async Task<Result<SearchResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var searchResponse = await client.SearchAsync<Law>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Content)
                        .Query(request.Content)
                        .Analyzer("serbian")))
                .Highlight(h => h
                        .Fields(fields => fields
                            .Add("content", new HighlightField
                            {
                                Type = HighlighterType.Plain
                            }))
                ), 
                cancellationToken);
            
            
            if (!searchResponse.IsValidResponse)
            {
                return Result.Fail("Something have gone wrong in basic search of laws!");
            }
            var response = CreateSearchResponse(searchResponse.Hits.ToList());
            return response;
        }

        private static SearchResponse CreateSearchResponse(List<Hit<Law>> hits)
        {
            var hitResponses = new List<HitResponse>();
            hits.ForEach(hit =>
            {
                if (hit.Highlight is not null && hit.Highlight.ContainsKey("content"))
                {
                    var highlightStrings = hit.Highlight["content"].ToList();
                    var highlight = "..." + string.Join(" ... ", highlightStrings) + "...";
                    var hitResponse = new HitResponse(highlight, string.IsNullOrEmpty(hit.Source?.FileName) ? "" : hit.Source.FileName);
                    hitResponses.Add(hitResponse);
                }

                if (hit.Highlight is null || !hit.Highlight.ContainsKey("content"))
                {
                    var highlight = hit.Source!.Content.Length > 150 
                        ? hit.Source.Content[..150] + "..."
                        : hit.Source.Content;
                    var hitResponse = new HitResponse(highlight, string.IsNullOrEmpty(hit.Source?.FileName) ? "" : hit.Source.FileName);
                    hitResponses.Add(hitResponse);
                }
            });
            return new SearchResponse(hitResponses);
        }
    }
}