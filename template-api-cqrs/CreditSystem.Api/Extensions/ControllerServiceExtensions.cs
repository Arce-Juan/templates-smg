using Template.Api.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Api.Extensions;

public static class ControllerServiceExtensions
{
    public static IServiceCollection AddTemplateSystemControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationExceptionFilter>();
        });
        return services;
    }
}
