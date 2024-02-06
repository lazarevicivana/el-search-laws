using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using MediatR;
using Search.Api.Contracts;

namespace Search.Api.Features.Contracts.Queries;

public static class GeoSearch
{
    public record Query(double Lat, double Long, long Distance) : IRequest<FluentResults.Result<ContractSearchResponse>>;
    internal class Handler(ElasticsearchClient client) : IRequestHandler<Query,FluentResults.Result<ContractSearchResponse>>
    {
        public async Task<FluentResults.Result<ContractSearchResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var latLongLocation = new LatLonGeoLocation
            {
                Lat = request.Lat,
                Lon = request.Long
            };
            var geoLocation =  GeoLocation.LatitudeLongitude(latLongLocation);
            var result = await client.SearchAsync<Contract>(s => s
                .Query(q => q
                    .GeoDistance(g => g
                        .Field(f => f.Location)
                        .Distance(request.Distance + "m")
                        .Location(geoLocation)
                    )
                ), cancellationToken);
            var res =  CreateSearchResponse(result.Hits.ToList());
            return res;
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
    }
}