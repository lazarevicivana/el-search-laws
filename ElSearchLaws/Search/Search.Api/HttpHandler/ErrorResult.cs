using FluentResults;

namespace Search.Api.HttpHandler;

public class ErrorResult : IError
{
    public string Message { get; }
    public Dictionary<string, object> Metadata { get; private set; }
    public List<IError> Reasons { get; }
    public int StatusCode { get; }

    private ErrorResult(string message)
    {
        Message = message;
        Metadata = new Dictionary<string, object>
        {
            { "Message: ", message }
        };
        Reasons = new List<IError> { new Error(message) };
    }

    public ErrorResult(string message, int code) : this(message)
    {
        StatusCode = code;
        Metadata.Add("StatusCode", code);
    }

    public ErrorResult(string message, int code, object metadata) : this(message, code)
    {
        Metadata.Add("AdditionalInfo", metadata);
    }
}