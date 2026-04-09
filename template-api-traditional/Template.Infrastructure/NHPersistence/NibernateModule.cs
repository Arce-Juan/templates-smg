using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using System.Reflection;
using Template.Application.Interfaces.Repositories;
using Template.Infrastructure.NHPersistence.Repositories;

namespace Template.Infrastructure.NHPersistence;

public static class NHibernateModule
{
    public static IServiceCollection AddNHibernate(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = new NHibernateOptions();
        return services.AddNHibernate(configuration, opt => { /* defaults */ });
    }

    public static IServiceCollection AddNHibernate(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<NHibernateOptions>? configureAction = null)
    {
        var options = new NHibernateOptions();
        configureAction?.Invoke(options);

        services.AddSingleton<ISessionFactory>(provider =>
        {
            return SessionFactory.GetSessionFactory(configuration, options);
        });

        services.AddScoped<ITemplateRepository, TemplateNHibernateRepository>();

        // Hosted service to ensure ISessionFactory is disposed on shutdown
        services.AddSingleton<IHostedService>(provider =>
        {
            var factory = provider.GetRequiredService<ISessionFactory>();
            return new NHibernateShutdownHostedService(factory);
        });

        return services;
    }
}

public class NHibernateOptions
{
    public bool ShowSql { get; set; } = true;
    public bool FormatSql { get; set; } = true;
    public string Dialect { get; set; } = typeof(NHibernate.Dialect.MsSql2012Dialect).AssemblyQualifiedName!;
    public string Driver { get; set; } = typeof(NHibernate.Driver.MicrosoftDataSqlClientDriver).AssemblyQualifiedName!;

    // Schema management during startup (use carefully in production)
    public bool UpdateSchema { get; set; } = false;
    public bool ValidateSchema { get; set; } = false;
    public bool SchemaActionShowSql { get; set; } = true;

    // Mapping assemblies to scan for hbm.xml or mapping attributes
    public List<Assembly>? MappingAssemblies { get; set; }
}

internal class NHibernateShutdownHostedService : IHostedService
{
    private readonly ISessionFactory _factory;

    public NHibernateShutdownHostedService(ISessionFactory factory)
    {
        _factory = factory;
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            (_factory as IDisposable)?.Dispose();
        }
        catch
        {
            // no-op
        }
        return Task.CompletedTask;
    }
}
