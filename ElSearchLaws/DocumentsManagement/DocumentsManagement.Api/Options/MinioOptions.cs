using Microsoft.Extensions.Options;

namespace DocumentsManagement.Api.MinioConfig;

public class MinioOptions : IValidateOptions<MinioOptions>
{
    public const string Minio = "Minio";
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string LawBucketName { get; set; } = string.Empty;
    public string ContractBucketName { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    
    public ValidateOptionsResult Validate(string? name, MinioOptions options)
    {
        if (string.IsNullOrEmpty(options.Endpoint))
            ValidateOptionsResult.Fail("Bucket name is undefined");
        return string.IsNullOrEmpty(options.Endpoint) ?
            ValidateOptionsResult.Fail("Endpoint is undefined") 
            : ValidateOptionsResult.Success;
    }
}