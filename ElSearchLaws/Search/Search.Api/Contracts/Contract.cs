using System;

namespace Search.Api.Contracts;

public class Contract
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = new();
}