using MassTransit;
using MediatR;
using Search.Api.Features.Contracts.Commands;
using Shared.Events.Contracts;

namespace Search.Api.MessageBroker;

public class ExportedContractMetadataConsumer : IConsumer<ContractMetadataExported>
{
    private readonly ILogger<ExportedContractMetadataConsumer> _logger;
    private readonly ISender _sender;

    public ExportedContractMetadataConsumer(ILogger<ExportedContractMetadataConsumer> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<ContractMetadataExported> context)
    {
        _logger.LogInformation($"Law received with name of {context.Message.FileName}",context.Message.FileName);
        await _sender.Send(new IndexContract.Command(context.Message));
    }
}