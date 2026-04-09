using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Application.Interfaces.Repositories;

namespace Template.Infrastructure.Persistence.Repositories;

public class TemplateRepository : ITemplateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TemplateRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TemplateEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Templates
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<TemplateEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Templates
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<TemplateEntity> CreateAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));

        await _dbContext.Templates.AddAsync(template, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return template;
    }

    public async Task UpdateAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));

        _dbContext.Templates.Update(template);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
