using Template.Domain.Entities;

namespace Template.Application.Interfaces.Repositories;

public interface ITemplateRepository
{
    Task<TemplateEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TemplateEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TemplateEntity> CreateAsync(TemplateEntity template, CancellationToken cancellationToken = default);
    Task UpdateAsync(TemplateEntity template, CancellationToken cancellationToken = default);
}
