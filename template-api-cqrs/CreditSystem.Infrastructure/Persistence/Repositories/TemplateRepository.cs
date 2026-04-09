using Template.Domain.Entities;
using Template.Domain.Enums;
using Template.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Template.Infrastructure.Persistence.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly TemplateSystemDbContext _context;

    public TemplateRepository(TemplateSystemDbContext context)
    {
        _context = context;
    }

    public async Task<TemplateEntitie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Templates
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(TemplateEntitie template, CancellationToken cancellationToken = default)
    {
        await _context.Templates.AddAsync(template, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(TemplateEntitie template, CancellationToken cancellationToken = default)
    {
        _context.Templates.Update(template);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
