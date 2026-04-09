using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Gateway;
using Template.Application.Interfaces.Repositories;
using Template.Infrastructure.Gateway;
using Template.Infrastructure.NHPersistence;
using Template.Infrastructure.NHPersistence.Repositories;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Persistence.Repositories;

namespace Template.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITemplateRepository, TemplateRepository>();

        // NHibernate configuration
        services.AddNHibernate(configuration);

        // Repository registration - using NHibernate implementation
        services.AddScoped<ITemplateRepository, TemplateNHibernateRepository>();

        services.AddApiGateway(configuration);

        return services;
    }

    public static IServiceCollection AddApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiGatewayOptions>(configuration.GetSection(ApiGatewayOptions.SectionName));
        services.AddHttpClient("ApiGateway");
        services.AddScoped<IApiGatewayClient, ApiGatewayClient>();
        return services;
    }
}
