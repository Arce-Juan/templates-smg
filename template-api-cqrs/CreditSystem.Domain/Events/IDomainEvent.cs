namespace Template.Domain.Events;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
