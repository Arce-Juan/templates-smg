using System.Net;

namespace Template.Application.Gateway;

public sealed class GatewayResponse<T>
{
    public bool IsSuccess { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public T? Data { get; init; }
    public string? ErrorContent { get; init; }

    public static GatewayResponse<T> Ok(HttpStatusCode statusCode, T? data) => new()
    {
        IsSuccess = true,
        StatusCode = statusCode,
        Data = data
    };

    public static GatewayResponse<T> Fail(HttpStatusCode statusCode, string? errorContent) => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        ErrorContent = errorContent
    };
}
