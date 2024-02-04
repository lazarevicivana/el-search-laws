using DocumentsManagement.Api.Common;
using FluentResults;
using MediatR;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace DocumentsManagement.Api.Features.Common;

public static class GetDocument
{
    public record Query(string FileName,
        string BucketName) : IRequest<Result<FileContent>>;

    internal class Handler(IMinioClient minioClient,
        ILogger<Handler> logger) : IRequestHandler<Query, Result<FileContent>>
    {
        public async Task<Result<FileContent>> Handle(Query request, CancellationToken cancellationToken)
        {
            var fileStream = new MemoryStream();
            var getObject = new GetObjectArgs()
                .WithBucket(request.BucketName)
                .WithObject(request.FileName)
                .WithCallbackStream(stream => {
                    stream.CopyTo(fileStream);
                });

            try {
                await minioClient.GetObjectAsync(getObject, cancellationToken);
            } catch (MinioException e) {
                logger.LogError("Error getting file: {@Message}", e.Message);
            }

            return new FileContent(fileStream, request.FileName);
        }
    }

}