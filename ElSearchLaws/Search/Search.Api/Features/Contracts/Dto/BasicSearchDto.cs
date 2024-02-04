namespace Search.Api.Features.Contracts.Dto;

public class BasicSearchDto : SearchCondition
{
    public  string Field { get; set; } = null!;
    public  string Value { get; set; } = null!;
    public  bool IsPhrase { get; set; }
}