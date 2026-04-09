using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace Template.Api.Extensions;

public sealed class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    public RabbitMqHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var host = _configuration["RabbitMQ:Host"] ?? "localhost";
        var port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672", System.Globalization.NumberStyles.None);
        var vHost = _configuration["RabbitMQ:VirtualHost"] ?? "/";
        var user = _configuration["RabbitMQ:Username"] ?? "guest";
        var password = _configuration["RabbitMQ:Password"] ?? "guest";

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = host,
                Port = port,
                VirtualHost = vHost,
                UserName = user,
                Password = password
            };
            using var connection = factory.CreateConnection();
            return Task.FromResult(connection.IsOpen
                ? HealthCheckResult.Healthy("RabbitMQ connection is open")
                : HealthCheckResult.Unhealthy("RabbitMQ connection is not open"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ unreachable", ex));
        }
    }
}
