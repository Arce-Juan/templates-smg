using System.Text;
using Template.Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Template.Api.Extensions;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddTemplateSystemAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
        var issuer = configuration["Jwt:Issuer"] ?? "TemplateSystem.Api";
        var audience = configuration["Jwt:Audience"] ?? "TemplateSystem.Client";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization();

        return services;
    }
}
