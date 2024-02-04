namespace Search.Api.Features.Contracts.Dto;

public class BoolQueryDto : SearchCondition
{
    public string Operator { get; set; } = null!;
    public List<SearchConditionWrapper> BoolQueryFields { get; set; } = null!;
}