namespace Template.Domain.Events;

public record TemplateConditionsCalculated(
    DateTimeOffset OccurredAt
) : IDomainEvent;
