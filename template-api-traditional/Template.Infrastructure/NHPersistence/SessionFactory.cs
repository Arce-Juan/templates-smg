using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Data;

namespace Template.Infrastructure.NHPersistence;

public static class SessionFactory
{
    private static ISessionFactory? _factory;
    private static readonly object _lock = new object();

    public static ISessionFactory GetSessionFactory(IConfiguration configuration, NHibernateOptions? options = null)
    {
        if (_factory == null)
        {
            lock (_lock)
            {
                if (_factory == null)
                {
                    _factory = CreateSessionFactory(configuration, options ?? new NHibernateOptions());
                }
            }
        }
        return _factory;
    }

    private static ISessionFactory CreateSessionFactory(IConfiguration configuration, NHibernateOptions options)
    {
        var cfg = new Configuration();

        // Database configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        cfg.DataBaseIntegration(db =>
        {
            db.ConnectionString = connectionString;
            db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            db.LogSqlInConsole = options.ShowSql;
            db.LogFormattedSql = options.FormatSql;
            db.IsolationLevel = IsolationLevel.ReadCommitted;
            db.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
            db.Driver<NHibernate.Driver.MicrosoftDataSqlClientDriver>();
        });
                    
        cfg.SetProperty(NHibernate.Cfg.Environment.Dialect, options.Dialect);
        cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, options.Driver);

        // Add mapping assemblies (allow override via options, otherwise include domain assembly)
        var mappingAssemblies = options.MappingAssemblies?.ToArray()
                               ?? new[] { typeof(Template.Domain.Entities.TemplateEntity).Assembly };

        foreach (var asm in mappingAssemblies)
        {
            cfg.AddAssembly(asm);
        }

        // Build session factory
        var sessionFactory = cfg.BuildSessionFactory();

        // Schema actions (optional)
        if (options.UpdateSchema)
        {
            // Execute schema update (applies changes to DB)
            var schemaUpdate = new SchemaUpdate(cfg);
            schemaUpdate.Execute(useStdOut: options.SchemaActionShowSql, doUpdate: true);
        }
        else if (options.ValidateSchema)
        {
            var schemaValidator = new SchemaValidator(cfg);
            schemaValidator.Validate();
        }

        return sessionFactory;
    }

    public static ISession OpenSession(IConfiguration configuration, NHibernateOptions? options = null)
    {
        return GetSessionFactory(configuration, options).OpenSession();
    }

    public static void Shutdown()
    {
        lock (_lock)
        {
            if (_factory != null)
            {
                try
                {
                    _factory.Close();
                    (_factory as IDisposable)?.Dispose();
                }
                finally
                {
                    _factory = null;
                }
            }
        }
    }
}
