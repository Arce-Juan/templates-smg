using Microsoft.OpenApi.Models;

namespace Template.Api.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddTemplateSystemSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Template System API",
                Version = "v1",
                Description = "API for creating and querying templates. Templates are processed asynchronously via message queue. All endpoints require a JWT; obtain one via POST /api/auth/token."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT token from POST /api/auth/token. Paste only the accessToken value (without 'Bearer ').",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }
}
