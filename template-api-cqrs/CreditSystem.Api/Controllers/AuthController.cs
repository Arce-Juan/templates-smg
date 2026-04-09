using Template.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;

    public AuthController(IJwtTokenGenerator tokenGenerator, IConfiguration configuration)
    {
        _tokenGenerator = tokenGenerator;
        _configuration = configuration;
    }

    /// <summary>
    /// Obtains a JWT access token for API calls. Use the returned accessToken in the Authorization header as Bearer {accessToken}.
    /// Credentials: clientId "credit", clientSecret "credit" (configurable in appsettings Jwt section).
    /// </summary>
    /// <response code="200">Token issued. Use data.accessToken in header: Authorization: Bearer &lt;token&gt;</response>
    /// <response code="401">Invalid clientId or clientSecret.</response>
    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        var clientId = _configuration["Jwt:ClientId"];
        var clientSecret = _configuration["Jwt:ClientSecret"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) ||
            request.ClientId != clientId || request.ClientSecret != clientSecret)
        {
            return Unauthorized();
        }

        var (token, expiresIn) = _tokenGenerator.GenerateAccessToken(request.ClientId);
        return Ok(new TokenResponse(token, expiresIn));
    }
}
