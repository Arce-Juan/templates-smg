using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.Application.Interfaces;
using Template.Application.Services;

namespace Template.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<IPersonasService, PersonasService>();

        return services;
    }
}