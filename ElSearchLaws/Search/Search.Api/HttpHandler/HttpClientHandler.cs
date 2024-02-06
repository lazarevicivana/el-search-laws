using System.Net.Http.Headers;
using FluentResults;
using NiceToGift.BuildingBlocks.Infrastructure.HttpHandler;

namespace Search.Api.HttpHandler;

public class HttpClientHandler : IHttpClientHandler
{
    private static readonly HttpClient _client = new();

    public async Task<Result<HttpResponseMessage>> SendAsync(HttpRequestMessage requestMessage, string? accessToken, string? tokenHeaderName)
    {
        requestMessage = SetAuthHeader(requestMessage, accessToken, tokenHeaderName);

        try
        {
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            return Result.Fail(HttpHandlerErrors.ConnectionError(message: response.ReasonPhrase!));
        }
        catch (Exception ex)
        {
            return Result.Fail(HttpHandlerErrors.ConnectionError(ex));
        }
    }

    private HttpRequestMessage SetAuthHeader(HttpRequestMessage requestMessage, string? accessToken, string? tokenHeaderName)
    {
        if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(tokenHeaderName))
        {
            if (tokenHeaderName != "Bearer")
            {
                requestMessage.Headers.Add(tokenHeaderName, accessToken);
            }
            else
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        return requestMessage;
    }
}
