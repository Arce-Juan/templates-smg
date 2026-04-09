using NHibernate;
using NHibernate.Linq;

namespace Template.Infrastructure.NHPersistence;

public class NHibernateRepository<T> where T : class
{
    protected readonly ISessionFactory _sessionFactory;

    public NHibernateRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public virtual async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.GetAsync<T>(id, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        return await session.Query<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        try
        {
            await session.SaveAsync(entity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return entity;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        try
        {
            await session.UpdateAsync(entity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return entity;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        using var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        
        try
        {
            await session.DeleteAsync(entity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
