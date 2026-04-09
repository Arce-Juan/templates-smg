using System.IdentityModel.Tokens.Jwt;
using Template.Application;
using Template.Infrastructure;
using Template.Infrastructure.Persistence;
using Template.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Serilog: bootstrap logger for startup, then full config from appsettings
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.File("logs/template-system-api-.txt", rollingInterval: RollingInterval.Day));

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTemplateSystemAuthentication(builder.Configuration);
builder.Services.AddTemplateSystemControllers();
builder.Services.AddTemplateSystemSwagger();
builder.Services.AddTemplateSystemHealthChecks(builder.Configuration);

var app = builder.Build();

// Apply pending EF Core migrations on startup (skip when running integration tests).
if (!string.Equals(app.Environment.EnvironmentName, "Testing", StringComparison.OrdinalIgnoreCase))
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<TemplateSystemDbContext>();
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Template System API v1"));
    // Redirect root to Swagger so http://localhost:8080/ opens the API docs
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/" || context.Request.Path == "")
        {
            context.Response.Redirect("/swagger/index.html", permanent: false);
            return;
        }
        await next();
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapTemplateSystemHealthChecks();

try
{
    Log.Information("Starting Template System API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
