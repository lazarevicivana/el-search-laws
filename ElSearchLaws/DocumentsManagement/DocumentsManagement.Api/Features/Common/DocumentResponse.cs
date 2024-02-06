namespace DocumentsManagement.Api.Features.Common;

public class DocumentResponse
{
    public Stream DocumentStream { get; set; } = null!;
    public string FileName { get; set; } = null!;
}