using Template.Application.Common;
using Template.Application.ValueObjects;

namespace Template.Application.Interfaces;

public interface ITemplateService
{
    Task<Response<TemplateDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Response<IReadOnlyList<TemplateDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Response<TemplateDto>> CreateAsync(TemplateDto template, CancellationToken cancellationToken = default);
    Task<Response<TemplateDto>> UpdateAsync(Guid id, TemplateDto template, CancellationToken cancellationToken = default);
}
