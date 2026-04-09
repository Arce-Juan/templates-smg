using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace Template.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddTemplateSystemHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHealthChecks()
            .AddSqlServer(connectionString!, name: "sqlserver", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "ready" })
            .AddCheck<RabbitMqHealthCheck>("rabbitmq", failureStatus: HealthStatus.Unhealthy, tags: new[] { "messaging", "ready" });

        return services;
    }

    public static IEndpointRouteBuilder MapTemplateSystemHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description })
                });
                await context.Response.WriteAsync(result);
            }
        });
        return endpoints;
    }
}
