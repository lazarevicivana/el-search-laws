namespace Shared.Events.Laws;

public record LawMetadataExported(
    string Title,
    string FileName,
    DateTime CreatedAt,
    string? Author,
    string? Category);
