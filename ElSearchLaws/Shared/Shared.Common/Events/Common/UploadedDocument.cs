using Shared.Events.Events.Common;

namespace Shared.Events.Common;

public record UploadedDocument(
    string FileName,
    string BucketName,
    DocumentType DocumentType);
