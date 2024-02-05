using MassTransit;
using MediatR;
using Search.Api.Features.Laws;
using Search.Api.Features.Laws.Commands;
using Shared.Events.Common;
using Shared.Events.Laws;

namespace Search.Api.MessageBroker;

public class ExportedLawMetadataConsumer : IConsumer<LawExported>
{
    private readonly ILogger<ExportedLawMetadataConsumer> _logger;
    private readonly ISender _sender;

    public ExportedLawMetadataConsumer(ILogger<ExportedLawMetadataConsumer> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<LawExported> context)
    {
        _logger.LogInformation($"Law received with name of {context.Message.Metadata.FileName}",context.Message.Metadata.FileName);
        await _sender.Send(new IndexLaw.Command(context.Message.Content,context.Message.Metadata));
    }
}