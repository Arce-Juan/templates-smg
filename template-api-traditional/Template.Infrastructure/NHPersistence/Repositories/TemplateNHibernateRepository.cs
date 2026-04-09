using NHibernate;
using NHibernate.Linq;
using Template.Application.Interfaces.Repositories;
using Template.Domain.Entities;

namespace Template.Infrastructure.NHPersistence.Repositories;

public class TemplateNHibernateRepository : ITemplateRepository
{
    private readonly ISessionFactory _sessionFactory;

    public TemplateNHibernateRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task<TemplateEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.GetAsync<TemplateEntity>(id, cancellationToken);
    }

    public async Task<IReadOnlyList<TemplateEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        var templates = await session.Query<TemplateEntity>()
            .ToListAsync(cancellationToken);
        
        return templates.AsReadOnly();
    }

    public async Task<TemplateEntity> CreateAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        if (template == null) 
            throw new ArgumentNullException(nameof(template));

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        try
        {
            await session.SaveAsync(template, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return template;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task UpdateAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        if (template == null) 
            throw new ArgumentNullException(nameof(template));

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        try
        {
            await session.UpdateAsync(template, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteAsync(TemplateEntity template, CancellationToken cancellationToken = default)
    {
        if (template == null) 
            throw new ArgumentNullException(nameof(template));

        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        try
        {
            await session.DeleteAsync(template, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
