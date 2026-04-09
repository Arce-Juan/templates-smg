using Template.Domain.Enums;

namespace Template.Domain.Entities;

public class TemplateEntitie
{
    public Guid Id { get; private set; }
    public TemplateStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private TemplateEntitie() { }

    public static TemplateEntitie Create()
    {
        return new TemplateEntitie
        {
            Id = Guid.NewGuid(),
            Status = TemplateStatus.Created,
            CreatedAt = DateTimeOffset.Now,
        };
    }

    public void SetStatus(TemplateStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
