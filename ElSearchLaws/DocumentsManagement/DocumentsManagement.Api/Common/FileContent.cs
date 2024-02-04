namespace DocumentsManagement.Api.Common;

public record FileContent(Stream Content,
    string FileName);