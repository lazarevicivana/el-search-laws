namespace Search.Api.Features.Contracts.Requests;

public class SearchContractRequest
{
    public string Field { get; set; } = null!;
    public string Value { get; set; } = null!;
}