using MediatR;
using Search.Api.Features.Laws.Queries;
using Search.Api.Features.Laws.Requests;
using Shared.Events.Events.Common;

namespace Search.Api.Features.Laws;

public static class LawsEndpoints
{
    private const string BaseUrl = "api/v1/law";
    
    public static void MapLawsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{BaseUrl}",
            async (SearchLawRequest request, ISender sender) =>
            {
                var result = await sender.Send(new BasicSearch.Query(
                    request.Query
                ));
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Conflict(result.Errors);

            }
        );
    }

}