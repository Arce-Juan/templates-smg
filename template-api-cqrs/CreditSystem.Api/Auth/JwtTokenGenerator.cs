using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Template.Api.Auth;

public interface IJwtTokenGenerator
{
    (string Token, int ExpiresInSeconds) GenerateAccessToken(string subject);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public (string Token, int ExpiresInSeconds) GenerateAccessToken(string subject)
    {
        var expiresInSeconds = _settings.ExpirationMinutes * 60;
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expiresInSeconds);
    }
}
