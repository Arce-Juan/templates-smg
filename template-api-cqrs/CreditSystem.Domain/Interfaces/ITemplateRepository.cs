using Template.Domain.Entities;

namespace Template.Domain.Interfaces;

public interface ITemplateRepository
{
    Task<TemplateEntitie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TemplateEntitie template, CancellationToken cancellationToken = default);
    Task UpdateAsync(TemplateEntitie template, CancellationToken cancellationToken = default);
}
