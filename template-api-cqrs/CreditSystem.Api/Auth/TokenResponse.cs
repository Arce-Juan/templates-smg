namespace Template.Api.Auth;

public record TokenResponse(string AccessToken, int ExpiresIn);
