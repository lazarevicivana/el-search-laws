using System;

namespace Search.Api.Laws;

public class Law
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = new();
    public string Title { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}