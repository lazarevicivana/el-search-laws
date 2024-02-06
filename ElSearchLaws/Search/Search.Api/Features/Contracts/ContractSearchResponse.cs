namespace Search.Api.Features.Contracts;

public record ContractSearchResponse(
    List<ContractHitResponse> Hits,
    long NumberOfResults);
public record ContractHitResponse(string GovernmentName,
    string GovernmentType,
    string SignatoryPersonName,
    string SignatoryPersonSurname,
    string Highlight,
    string Content,
    string FileName,
    Guid ContractId);