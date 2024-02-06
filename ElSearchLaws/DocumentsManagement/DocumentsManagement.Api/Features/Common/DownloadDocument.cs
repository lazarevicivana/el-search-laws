using DocumentsManagement.Api.MinioConfig;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Shared.Events.Common;

namespace DocumentsManagement.Api.Features.Common;

public static class DownloadDocument
{
    public record Query(DocumentType Type,
        string FileName
       ) : IRequest<Result<DocumentResponse>>;
    internal class Handler(IMinioClient minioClient,
        ILogger<Handler> logger,
        IOptions<MinioOptions> minioOptions
        ): IRequestHandler<Query,Result<DocumentResponse>>
    {
        public async Task<Result<DocumentResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            switch (request.Type)
            {
                case DocumentType.Contract:
                    return  await GetDocumentFromDb(request.FileName, minioOptions.Value.ContractBucketName,cancellationToken);
               default:
                    return await GetDocumentFromDb(request.FileName, minioOptions.Value.LawBucketName,cancellationToken);
            }
        }

        private async Task<DocumentResponse> GetDocumentFromDb(string fileName, string bucketName,
            CancellationToken cancellationToken)
        {
            var fileStream = new MemoryStream();
            var getObject = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => { stream.CopyTo(fileStream); });

            try
            {
                await minioClient.GetObjectAsync(getObject, cancellationToken);
            }
            catch (MinioException e)
            {
                logger.LogError("Error getting file: {@Message}", e.Message);
            }

            var response = new DocumentResponse
            {
                DocumentStream = fileStream,
                FileName = fileName
            };
            return response;
        }
    }
}