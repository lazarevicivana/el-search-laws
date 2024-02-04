namespace Search.Api.Features.Contracts.Dto;

public class SearchConditionWrapper
{
    public BasicSearchDto? BasicSearchDto { get; set; }
    public BoolQueryDto BoolQueryDto { get; set; } = null!;

    public SearchCondition GetCondition()
    {
        if (BasicSearchDto is not null)
            return BasicSearchDto;
        return BoolQueryDto;
    }
}