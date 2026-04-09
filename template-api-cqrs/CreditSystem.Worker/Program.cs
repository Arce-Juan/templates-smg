using Template.Worker.Extensions;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.AddWorkerLogging();
builder.AddWorkerServices();

var host = builder.Build();

try
{
    Log.Information("Starting Template System Worker");
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Worker terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
