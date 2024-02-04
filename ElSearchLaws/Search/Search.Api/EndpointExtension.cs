using Search.Api.Features.Contracts;
using Search.Api.Features.Laws;

namespace Search.Api;

public static class EndpointExtension
{
    public static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder application)
    {
        application.MapLawsEndpoints();
        application.MapContractEndpoints();
        return application;
    }
}