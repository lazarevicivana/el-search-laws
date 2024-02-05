using Elastic.Clients.Elasticsearch;
using MediatR;
using Search.Api.Contracts;
using Shared.Events.Contracts;

namespace Search.Api.Features.Contracts.Commands;

public static class IndexContract
{
    public record Command(ContractMetadataExported ContractMetadataExported) : IRequest;
    internal class Handler(
        ElasticsearchClient client,
        ILogger<Handler> logger): IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var contract = new Contract
            {
                GovernmentName =request.ContractMetadataExported.Government?.Name!,
                GovernmentType = request.ContractMetadataExported.Government?.AdministrationLevel!,
                SignatoryPersonName = request.ContractMetadataExported.AgencySignature?.EmployeeName!,
                SignatoryPersonSurname = request.ContractMetadataExported.AgencySignature?.EmployeeSurname!,
                Id = Guid.NewGuid(),
                Content = request.ContractMetadataExported.Content,
                CreatedAt = new DateTime()
            };
            var response = await client.IndexAsync(contract, cancellationToken);
            if(!response.IsValidResponse)
                logger.LogError("Failed to index contract: {DebugInformation}",response.DebugInformation);
        }
    }
}