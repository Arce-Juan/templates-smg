using Template.Application;
using Template.Infrastructure;
using Template.Worker.Consumers;
using MassTransit;
using Serilog;

namespace Template.Worker.Extensions;

public static class WorkerApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddWorkerLogging(this IHostApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.File("logs/template-system-worker-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger, dispose: true);

        return builder;
    }

    public static IHostApplicationBuilder AddWorkerServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration,
            configureBus: bus =>
            {
                bus.AddConsumer<TemplateCreatedConsumer>();
            },
            configureReceiveEndpoints: (rabbit, context) =>
            {
                rabbit.ReceiveEndpoint("template-created", e => e.ConfigureConsumer<TemplateCreatedConsumer>(context));
            });

        builder.Services.AddHostedService<Worker>();

        return builder;
    }
}
