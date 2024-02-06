using DocumentsManagement.Api.Features.Common;
using FluentResults;
using MassTransit;
using MediatR;
using Minio;
using Minio.DataModel.Args;
using Shared.Events.Common;

namespace DocumentsManagement.Api.Features.Contracts.Commands;

public static class UploadContract
{
    public record Command(IFormFile File,
        string BucketName,
        DocumentType DocumentType): IRequest<Result>;
    
    internal class Handler(
        IMinioClient minioClient,
        ILogger<Handler> logger,
        IPublishEndpoint eventBus,
        ISender sender
    ): IRequestHandler<Command,Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var putArgs = new PutObjectArgs()
                .WithObject(request.File.FileName)
                .WithObjectSize(request.File.Length)
                .WithStreamData(request.File.OpenReadStream())
                .WithBucket(request.BucketName);
            try
            {
                await minioClient.PutObjectAsync(
                    putArgs,
                    cancellationToken
                ).ConfigureAwait(false);
                
                await sender.Send(new SendDocumentToProcessing.Query(
                        request.File,
                        request.DocumentType), 
                    cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError("Error uploading file: {Message}", e.Message);
                return Result.Fail("Error uploading file");
            }

            return Result.Ok();
        }
    }
}