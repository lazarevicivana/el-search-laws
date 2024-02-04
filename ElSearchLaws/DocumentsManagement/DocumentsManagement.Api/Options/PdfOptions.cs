using Microsoft.Extensions.Options;

namespace DocumentsManagement.Api.Options;

public class PdfOptions : IValidateOptions<PdfOptions>
{
    public const string Pdf = nameof(Pdf);
    public string License { get; init; } = string.Empty;

    public ValidateOptionsResult Validate(string? name, PdfOptions options) =>
        string.IsNullOrWhiteSpace(options.License)
            ? ValidateOptionsResult.Fail("License is required.")
            : ValidateOptionsResult.Success;
}