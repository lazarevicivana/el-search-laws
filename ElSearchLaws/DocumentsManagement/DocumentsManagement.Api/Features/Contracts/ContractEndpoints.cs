using DocumentsManagement.Api.Features.Contracts.Commands;
using DocumentsManagement.Api.MinioConfig;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Events.Events.Common;

namespace DocumentsManagement.Api.Features.Contracts;

public static class ContractEndpoints
{
    private const string BaseUrl = "api/v1/contract";
    public static void MapContractsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{BaseUrl}",
            async (IFormFile file, ISender sender, IOptions<MinioOptions> options) =>
            {
                var minioOptions = options.Value;
                var result = await sender.Send(new UploadContract.Command(
                    file,
                    minioOptions.ContractBucketName,
                    DocumentType.Contract
                ));
                return result.IsSuccess
                    ? Results.Ok()
                    : Results.Conflict(result.Errors);

            }
        ).DisableAntiforgery();
    }
}