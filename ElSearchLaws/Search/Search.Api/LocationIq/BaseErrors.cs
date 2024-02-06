using Search.Api.HttpHandler;

namespace Search.Api.LocationIq;

public static class BaseErrors
{
    public static ErrorResult InvalidData(string message = ErrorCodes.InvalidDataMessage)
    {
        return new ErrorResult(message, ErrorCodes.InvalidData);
    }

    public static ErrorResult Forbidden(string message = ErrorCodes.ForbiddenMessage)
    {
        return new ErrorResult(message, ErrorCodes.Forbidden);
    }

    public static ErrorResult NotFound(string message = ErrorCodes.NotFoundMessage)
    {
        return new ErrorResult(message, ErrorCodes.NotFound);
    }

    public static ErrorResult Conflict(string message = ErrorCodes.ConflictMessage)
    {
        return new ErrorResult(message, ErrorCodes.Conflict);
    }

    public static ErrorResult Internal(object exception, string message = ErrorCodes.InternalMessage)
    {
        return new ErrorResult(message, ErrorCodes.Internal, exception);
    }
}