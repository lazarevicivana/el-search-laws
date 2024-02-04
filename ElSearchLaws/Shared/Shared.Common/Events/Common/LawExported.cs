using Shared.Events.Laws;

namespace Shared.Events.Common;

public record LawExported(
    string Content,
    LawMetadataExported Metadata);
