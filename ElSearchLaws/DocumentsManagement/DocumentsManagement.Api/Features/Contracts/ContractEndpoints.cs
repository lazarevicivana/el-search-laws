using DocumentsManagement.Api.Features.Common;
using DocumentsManagement.Api.Features.Contracts.Commands;
using DocumentsManagement.Api.MinioConfig;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Events.Common;

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
        app.MapGet($"{BaseUrl}/{{fileName}}/{{type}}", async (string fileName, DocumentType type, ISender sender) =>
        {
            var result = await sender.Send(new DownloadDocument.Query(type, fileName));
            if (result.IsSuccess)
                result.Value.DocumentStream.Position = 0;
            return result.IsSuccess
                ? Results.File(result.Value.DocumentStream, "application/pdf", fileName)
                : Results.BadRequest();
        });
    }
}