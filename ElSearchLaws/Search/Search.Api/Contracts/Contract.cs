﻿using System;

namespace Search.Api.Contracts;

public class Contract
{
    public Guid Id { get; set; }
    public string GovernmentName { get; set; } = string.Empty;
    public string GovernmentType { get; set; } = string.Empty;
    public string SignatoryPersonSurname { get; set; } = string.Empty;
    public string SignatoryPersonName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = new();
}