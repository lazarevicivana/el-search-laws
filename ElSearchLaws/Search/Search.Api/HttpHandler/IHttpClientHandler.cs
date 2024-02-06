using FluentResults;

namespace NiceToGift.BuildingBlocks.Infrastructure.HttpHandler;

public interface IHttpClientHandler
{
    Task<Result<HttpResponseMessage>> SendAsync(HttpRequestMessage requestMessage, string? accessToken = null, string? tokenHeaderName = "Bearer");
}
