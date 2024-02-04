using Microsoft.Extensions.Options;

namespace Shared.Options;

public class RabbitMqOptions: IValidateOptions<RabbitMqOptions>
{
    public const string RabbitMq = "RabbitMq";
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public ValidateOptionsResult Validate(string? name, RabbitMqOptions options)
    {
        if(string.IsNullOrEmpty(options.Host))
            return ValidateOptionsResult.Fail("Host is undefined");
        if(string.IsNullOrEmpty(options.Password))
            return ValidateOptionsResult.Fail("Password is undefined");
        return string.IsNullOrEmpty(options.Username)
            ? ValidateOptionsResult.Fail("Username is not defined")
            : ValidateOptionsResult.Success;
    }
}