using DocumentsManagement.Api.Features.Laws.Commands;
using DocumentsManagement.Api.MinioConfig;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Events.Common;

namespace DocumentsManagement.Api.Features.Laws;

internal static class LawsEndpoints
{
    private const string BaseUrl = "api/v1/law";
    public static void MapLawsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{BaseUrl}",
            async (IFormFile file, ISender sender, IOptions<MinioOptions> options) =>
            {
                var minioOptions = options.Value;
                var result = await sender.Send(new UploadLaw.Command(
                    file,
                    minioOptions.LawBucketName,
                    DocumentType.Law
                ));
                return result.IsSuccess
                    ? Results.Ok()
                    : Results.Conflict(result.Errors);

            }
            ).DisableAntiforgery();
    }
    
}