using MassTransit;
using Shared.Events.Common;

namespace Search.Api.MessageBroker;

public class UploadedDocumentConsumer : IConsumer<UploadedDocument>
{
    private readonly ILogger<UploadedDocumentConsumer> _logger;

    public UploadedDocumentConsumer(ILogger<UploadedDocumentConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UploadedDocument> context)
    {
        _logger.LogInformation($"Document of type {context.Message.DocumentType} received!", context.Message.DocumentType);
        return Task.CompletedTask;
    }
}