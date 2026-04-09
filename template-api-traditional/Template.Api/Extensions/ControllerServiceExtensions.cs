using Template.Api.Filters;

namespace Template.Api.Extensions;

public static class ControllerServiceExtensions
{
    public static IServiceCollection AddCreditSystemControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationExceptionFilter>();
        });
        return services;
    }
}
