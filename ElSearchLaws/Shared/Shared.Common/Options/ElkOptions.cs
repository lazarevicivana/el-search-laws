using Microsoft.Extensions.Options;

namespace Shared.Options;

public class ElkOptions : IValidateOptions<ElkOptions>
{
    public const string Elk = "Elk";
    public string HttpSinkRequestUri { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public ValidateOptionsResult Validate(string? name, ElkOptions options)
    {
        if(string.IsNullOrEmpty(options.ServiceName))
            return ValidateOptionsResult.Fail("Service name is not defined!");
        return string.IsNullOrEmpty(options.HttpSinkRequestUri)
            ? ValidateOptionsResult.Fail("Sink request Uri is not defined!")
            : ValidateOptionsResult.Success;

    }
}