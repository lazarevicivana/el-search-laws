using Elastic.Clients.Elasticsearch;
using MediatR;
using Search.Api.Laws;
using Shared.Events.Laws;

namespace Search.Api.Features.Laws.Commands;

public static class IndexLaw
{
    public record Command(string Content,
        LawMetadataExported Metadata) : IRequest;
    internal class Handler(
        ElasticsearchClient client,
        ILogger<Handler> logger): IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var law = new Law
            {
                Content = request.Content,
                Title = request.Metadata.Title,
                FileName = request.Metadata.FileName,
                Id = Guid.NewGuid()
            };
            var response = await client.IndexAsync(law, cancellationToken);
            if(!response.IsValidResponse)
                logger.LogError("Failed to index law: {DebugInformation}",response.DebugInformation);
        }
    }
}