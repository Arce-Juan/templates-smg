using System.Net;
using System.Text.Json.Serialization;

namespace Template.Application.Common;

/// <summary>
/// Envelope for operation results (Content + StatusCode as single source of truth).
/// Handlers always return Response<> base controller maps StatusCode to IActionResult.
/// </summary>
public class Response<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }

    /// <summary>
    /// Alias for Data. Not serialized to avoid duplicating payload.
    /// </summary>
    [JsonIgnore]
    public T? Content => Data;
    public string? Type { get; init; }
    public string? Title { get; init; }
    public string? Detail { get; init; }

    /// <summary>
    /// HTTP status for the response (drives base controller mapping to IActionResult).
    /// </summary>
    public HttpStatusCode? StatusCode { get; init; }
    public List<ValidationError>? Errors { get; init; }

    public static Response<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message,
        StatusCode = HttpStatusCode.OK
    };

    public static Response<T> Failure(
        string message,
        string? type = null,
        string? title = null,
        string? detail = null,
        HttpStatusCode? statusCode = null,
        List<ValidationError>? errors = null) => new()
        {
            Success = false,
            Message = message,
            Type = type,
            Title = title,
            Detail = detail,
            StatusCode = statusCode ?? HttpStatusCode.BadRequest,
            Errors = errors
        };

    /// <summary>
    /// Builds a failure response for validation errors (e.g. from FluentValidation).
    /// </summary>
    public static Response<T> ValidationFailure(string message, List<ValidationError> errors) => new()
    {
        Success = false,
        Message = message,
        Title = "Error de validación",
        Type = "validation-error",
        Errors = errors,
        StatusCode = HttpStatusCode.BadRequest
    };
}

/// <summary>
/// Single validation error (field-level).
/// </summary>
public record ValidationError(string PropertyName, string Message, string? Code = null);
