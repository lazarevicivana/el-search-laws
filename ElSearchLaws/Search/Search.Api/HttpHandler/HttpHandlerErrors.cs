namespace Search.Api.HttpHandler;

internal static class HttpHandlerErrors
{
    private const int ConnectionErrorCode = 503;
    private const string ConnectionErrorMessage = "Error in connecting to remote service.";

    public static ErrorResult ConnectionError(object? metadata = null, string message = ConnectionErrorMessage)
    {
        return new ErrorResult(message, ConnectionErrorCode, metadata);
    }
}
