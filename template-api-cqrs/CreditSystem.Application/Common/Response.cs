using System.Net;
using System.Text.Json.Serialization;

namespace Template.Application.Common;

public class Response<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public string? Type { get; init; }
    public string? Title { get; init; }
    public string? Detail { get; init; }

    public HttpStatusCode? StatusCode { get; init; }
    public List<ValidationError>? Errors { get; init; }

    public static Response<T> Ok(T? data, string? message = null, HttpStatusCode? statusCode = null) => new()
    {
        Success = true,
        Data = data,
        Message = message,
        StatusCode = statusCode ?? HttpStatusCode.OK
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

    public static Response<T> ValidationFailure(string message, List<ValidationError> errors) => new()
    {
        Success = false,
        Message = message,
        Title = ErrorMessage.Validation.Title,
        Type = "validation-error",
        Errors = errors,
        StatusCode = HttpStatusCode.BadRequest
    };
}
