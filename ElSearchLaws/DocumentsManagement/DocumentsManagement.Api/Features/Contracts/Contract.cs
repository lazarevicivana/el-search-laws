namespace DocumentsManagement.Api.Features.Contracts;

public class Contract
{
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ContractAddress? AgencyAddress { get; set; }
    public ContractAddress? ClientAddress { get; set; }
    public string? AgencyEmail { get; set; }
    public string? AgencyPhoneNumber { get; set; }
    public SignatureExtracted? AgencySign { get; set; }
    public string? ClientName { get; set; }
    public string? ClientLevel { get; set; }
    public string? ClientEmail { get; set; }
    public string? ClientPhoneNumber { get; set; }
    public SignatureExtracted? ClientSign { get; set; }
    public string Content { get; set; } = null!;

    public ContentMetadata? Metadata { get; set; }
}
public record ContractAddress(string? Street, string? Number, string? City, string? FullAddress);

public record SignatureExtracted(string? EmployeeName, string? EmployeeSurname, string? EmployeeFullName);
public record ContentMetadata(
    string Title,
    string FileName,
    DateTime CreatedAt,
    string? Author,
    string? Category
);