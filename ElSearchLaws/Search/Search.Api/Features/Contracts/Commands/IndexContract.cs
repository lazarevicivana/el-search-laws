using Elastic.Clients.Elasticsearch;
using MediatR;
using Search.Api.Contracts;
using Search.Api.LocationIq;
using Shared.Events.Contracts;

namespace Search.Api.Features.Contracts.Commands;

public static class IndexContract
{
    public record Command(ContractMetadataExported ContractMetadataExported) : IRequest;
    internal class Handler(
        ILocationClient locationClient,
        ElasticsearchClient client,
        ILogger<Handler> logger): IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var address = new Address(request.ContractMetadataExported.Government?.Address!.Number!,
                request.ContractMetadataExported.Government?.Address!.Street!,
                request.ContractMetadataExported.Government?.Address!.City!, 
                "Serbia");
            var locationResult =  await locationClient.GetAddressLocation(address);
          
            var contract = new Contract
            {
                GovernmentName =request.ContractMetadataExported.Government?.Name!,
                GovernmentType = request.ContractMetadataExported.Government?.AdministrationLevel!,
                SignatoryPersonName = request.ContractMetadataExported.AgencySignature?.EmployeeName!,
                SignatoryPersonSurname = request.ContractMetadataExported.AgencySignature?.EmployeeSurname!,
                Id = Guid.NewGuid(),
                Content = request.ContractMetadataExported.Content,
                CreatedAt = new DateTime(),
                FileName = request.ContractMetadataExported.FileName,
            };
            if (locationResult.IsSuccess)
            {
                var latLonLocation = new LatLonGeoLocation
                {
                    Lat = locationResult.Value.Latitude,
                    Lon = locationResult.Value.Longitude
                };
                contract.Location = latLonLocation;
            }
            
            var response = await client.IndexAsync(contract, cancellationToken);
            if(!response.IsValidResponse)
                logger.LogError("Failed to index contract: {DebugInformation}",response.DebugInformation);
        }
    }
}