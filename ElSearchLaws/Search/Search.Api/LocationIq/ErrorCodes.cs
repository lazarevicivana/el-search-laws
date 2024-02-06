namespace Search.Api.LocationIq;

public static class ErrorCodes
{
    public const int InvalidData = 400;
    public const string InvalidDataMessage = "Invalid data supplied.";

    public const int Forbidden = 403;
    public const string ForbiddenMessage = "Access to resource restricted.";

    public const int NotFound = 404;
    public const string NotFoundMessage = "Accessed resource not found.";

    public const int Conflict = 409;
    public const string ConflictMessage = "Persistence conflict error.";

    public const int Internal = 500;
    public const string InternalMessage = "Internal server error.";
}