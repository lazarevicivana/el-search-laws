using Microsoft.Extensions.Options;

namespace Search.Api.ElasticSearch.Options;

public class ElasticSearchOptions : IValidateOptions<ElasticSearchOptions>
{
    public const string ElasticSearch = "ElasticSearch";
    public string Uri { get; set; } = string.Empty;
    public string SrbLawIndex { get; set; } = string.Empty;
    public string SrbContractIndex { get; set; } = string.Empty;

    public ValidateOptionsResult Validate(string? name, ElasticSearchOptions options)
    {
        if(string.IsNullOrWhiteSpace(options.Uri))
            return ValidateOptionsResult.Fail("Elastic search uri is not defined");
        if(string.IsNullOrWhiteSpace(options.SrbContractIndex))
            return ValidateOptionsResult.Fail("Elastic search contract index is not defined");
        return string.IsNullOrWhiteSpace(options.SrbLawIndex) ? 
            ValidateOptionsResult.Fail("Elastic search law index is not defined") 
            : ValidateOptionsResult.Success;
    }
}