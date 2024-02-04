using DocumentsManagement.Api.Features.Common;
using FluentResults;
using MassTransit;
using MediatR;
using Shared.Events.Common;
using Shared.Events.Laws;

namespace DocumentsManagement.Api.Features.Laws.Commands;

public static class ProcessLaw
{
    public record Command(IFormFile File) : IRequest<Result>;

    internal class Handler(
        IContentExtractor<Law> contentExtractor,
        IPublishEndpoint eventBus) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var law = await contentExtractor
                .ExtractContentAsync(request.File, cancellationToken);
            
            if (law.IsFailed) {
                return Result.Fail("Failed to process law");
            }

            var lawMetadata = law.Value.Metadata;
            var metadata = new LawMetadataExported(
                lawMetadata.Title,
                lawMetadata.FileName,
                lawMetadata.CreatedAt,
                lawMetadata.Author,
                lawMetadata.Category
            );
            await eventBus.Publish(
                new LawExported(
                    law.Value.Content,
                    metadata
                ),
                cancellationToken
            );
            return Result.Ok();
        }
    }
}