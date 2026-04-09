using Template.Application.Interfaces;
using Template.Domain.Events;
using Template.Domain.Interfaces;
using Template.Infrastructure.Messaging;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Infrastructure;

public static class DependencyInjection
{
    /// <param name="configureBus">Register consumers (AddConsumer). Called during service registration; do not use for ReceiveEndpoint.</param>
    /// <param name="configureReceiveEndpoints">Configure RabbitMQ receive endpoints (ReceiveEndpoint). Called when the bus is configured.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureBus = null,
        Action<IRabbitMqBusFactoryConfigurator, IRegistrationContext>? configureReceiveEndpoints = null)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<TemplateSystemDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ITemplateRepository, TemplateRepository>();

        var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitVirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
        var rabbitUsername = configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitPassword = configuration["RabbitMQ:Password"] ?? "guest";
        var rabbitPort = configuration["RabbitMQ:Port"] ?? "5672";

        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost, ushort.Parse(rabbitPort), rabbitVirtualHost, h =>
                {
                    h.Username(rabbitUsername);
                    h.Password(rabbitPassword);
                });
                // Un solo exchange por evento (nombre corto). Sin esto, MassTransit crea además exchanges con el tipo completo (TemplateSystem.Domain.Events:TemplateCreated).
                cfg.Message<TemplateCreated>(m => m.SetEntityName("template-created"));
                cfg.Message<TemplateConditionsCalculated>(m => m.SetEntityName("template-conditions-calculated"));
                configureReceiveEndpoints?.Invoke(cfg, context);
            });
        });

        services.AddScoped<IEventBus, MassTransitEventBus>();

        return services;
    }
}
