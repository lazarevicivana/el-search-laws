namespace DocumentsManagement.Api.Features.Laws;

public record Law(string Content, LawMetadata Metadata);
public record LawMetadata(
    string Title,
    string FileName,
    DateTime CreatedAt,
    string? Author,
    string? Category
);