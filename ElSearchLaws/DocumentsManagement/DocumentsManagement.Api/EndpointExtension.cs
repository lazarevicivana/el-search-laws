using DocumentsManagement.Api.Features.Contracts;
using DocumentsManagement.Api.Features.Laws;

namespace DocumentsManagement.Api;

public static class EndpointExtension
{
    public static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder application)
    {
        application.MapLawsEndpoints();
        application.MapContractsEndpoints();
        return application;
    }
}