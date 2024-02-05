namespace Shared.Events.Contracts;

public class ContractMetadataExported
{
    public GovernmentExported? Government { get; set; }
    public SignatureExported? GovernmentSignature { get; set; }
    public AgencyExported? Agency { get; set; }
    public SignatureExported? AgencySignature { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string Content { get; set; } = null!;
    public ContentMetadataExported ContentMetadataExported { get; set; } = null!;
    public string FileName { get; set; } = null!;
}

public record AgencyExported(string? Phone, string? Email, AddressExported? Address);

public record SignatureExported(string? EmployeeName, string? EmployeeSurname, string? EmployeeFullName);

public record GovernmentExported(string? Name, string? AdministrationLevel, string? Phone, string? Email, AddressExported? Address);

public record AddressExported(string? Street, string? Number, string? City);

public record ContentMetadataExported(
    string? Title,
    string? FileName,
    DateTime CreatedAt,
    string? Author,
    string? Category
);