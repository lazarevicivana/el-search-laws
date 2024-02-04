using DocumentsManagement.Api.Common;
using FluentResults;

namespace DocumentsManagement.Api.Features.Common;

public interface IContentExtractor<T> where T : class
{
    Task<Result<T>> ExtractContentAsync(IFormFile file, CancellationToken cancellationToken);
}