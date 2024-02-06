using DocumentsManagement.Api.Features.Contracts.Commands;
using DocumentsManagement.Api.Features.Laws.Commands;
using FluentResults;
using MediatR;
using Shared.Events.Common;

namespace DocumentsManagement.Api.Features.Common;

public static class SendDocumentToProcessing
{
    public record Query(IFormFile File,
        DocumentType DocumentType) : IRequest<Result>;
    
    internal class Handler(ISender sender,
        ILogger<Handler> logger): IRequestHandler<Query,Result>
    {
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Sending to indexing...");
            switch (request.DocumentType)
            {
                case DocumentType.Contract:
                    await sender.Send(new ProcessContract.Command(request.File), cancellationToken);
                    break;
                case DocumentType.Law:
                    await sender.Send(new ProcessLaw.Command(request.File),cancellationToken);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Result.Ok();
        }
    }
}