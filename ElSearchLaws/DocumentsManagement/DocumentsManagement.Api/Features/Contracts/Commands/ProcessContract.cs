using DocumentsManagement.Api.Features.Common;
using FluentResults;
using MassTransit;
using MediatR;
using Shared.Events.Contracts;

namespace DocumentsManagement.Api.Features.Contracts.Commands;

public static class ProcessContract
{
    public record Command(IFormFile File) : IRequest<Result>;
    
    internal class Handler( IContentExtractor<Contract> contentExtractor,
        IPublishEndpoint eventBus): IRequestHandler<Command,Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var contract = await contentExtractor
                .ExtractContentAsync(request.File, cancellationToken);
            if (contract.IsFailed) {
                return Result.Fail("Failed to process law");
            }

            var content = contract.Value!;
            var contractExported = CreateContractMetadataExported(content,request.File.FileName);
            await eventBus.Publish(
                contractExported,
                cancellationToken
            );
            return Result.Ok();
        }

        private ContractMetadataExported CreateContractMetadataExported(Contract contract,string fileName)
        {
            var government = new GovernmentExported(
                Name: contract.ClientName,
                AdministrationLevel: contract.ClientLevel,
                Phone: contract.ClientPhoneNumber,
                Email: contract.ClientEmail,
                Address: new AddressExported(
                    Street: contract.ClientAddress?.Street,
                    Number: contract.ClientAddress?.Number,
                    City: contract.ClientAddress?.City
                )
            );
            var governmentSignature = new SignatureExported(
                EmployeeName: contract.ClientSign?.EmployeeName,
                EmployeeSurname: contract.ClientSign?.EmployeeSurname,
                EmployeeFullName: contract.ClientSign?.EmployeeFullName
            );
            var agency = new AgencyExported(
                Phone: contract.AgencyPhoneNumber,
                Email: contract.AgencyEmail,
                Address: new AddressExported(
                    Street: contract.AgencyAddress?.Street,
                    Number: contract.AgencyAddress?.Number,
                    City: contract.AgencyAddress?.City
                )
            );
            var agencySignature = new SignatureExported(
                EmployeeName: contract.AgencySign?.EmployeeName,
                EmployeeSurname: contract.AgencySign?.EmployeeSurname,
                EmployeeFullName: contract.AgencySign?.EmployeeFullName
            );
            var metadata = new ContentMetadataExported(
                Title: contract.Metadata?.Title,
                FileName: contract.Metadata?.FileName,
                CreatedAt: contract.Metadata?.CreatedAt ?? default(DateTime),
                Author: contract.Metadata?.Author,
                Category: contract.Metadata?.Category
            );

           return new ContractMetadataExported
            {
                Government = government,
                ContentMetadataExported = metadata,
                Agency = agency,
                AgencySignature = agencySignature,
                GovernmentSignature = governmentSignature,
                CreatedAt = new DateTime(),
                Content = contract.Content,
                FileName = fileName
            };
        }
    }
}