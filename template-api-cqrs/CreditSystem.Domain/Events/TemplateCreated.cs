namespace Template.Domain.Events;

public record TemplateCreated(
    Guid RequestId,
    DateTimeOffset OccurredAt
) : IDomainEvent;
