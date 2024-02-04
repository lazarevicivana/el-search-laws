using MediatR;
using Search.Api.Features.Contracts.Queries;
using Search.Api.Features.Contracts.Requests;

namespace Search.Api.Features.Contracts;

public static class ContractEndpoints
{
    private const string BaseUrl = "api/v1/contract";
    
    public static void MapContractEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{BaseUrl}",
            async (SearchContractRequest request, ISender sender) =>
            {
                var result = await sender.Send(new BasicSearch.Query(
                    request.Field,
                    request.Value
                ));
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Conflict(result.Errors);

            }
        );
        app.MapPost($"{BaseUrl}/bool",async (SearchContractRequest request, ISender sender) =>
        {
           var result = await sender.Send(new BoolSearch.Query(
                "content:\"Radio televizija\" AND governmentName:Republika"));
           return result.IsSuccess
               ? Results.Ok(result.Value)
               : Results.Conflict(result.Errors);
        });
    }
}