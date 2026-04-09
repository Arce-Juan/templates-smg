using DotNetEnv;
using FluentValidation;
using Serilog;
using Template.Api.Extensions;
using Template.Application.Common;
using Template.Application.Extensions;
using Template.Infrastructure.Extensions;

LoadLocalEnvIfDevelopment();

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog desde la extensión (mueve la lógica aquí para mantener Program.cs limpio)
builder.ConfigureSerilog();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(typeof(Response<object>).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DI using extension methods
builder.Services.AddCreditSystemControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

try
{
    Log.Information("Starting up");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

static void LoadLocalEnvIfDevelopment()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    if (!string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase))
        return;

    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null)
    {
        var path = Path.Combine(dir.FullName, ".env");
        if (File.Exists(path))
        {
            Env.Load(path);
            return;
        }

        dir = dir.Parent;
    }
}
