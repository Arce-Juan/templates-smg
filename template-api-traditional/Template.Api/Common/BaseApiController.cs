using Microsoft.AspNetCore.Mvc;
using System.Net;
using Template.Application.Common;

namespace Template.Api.Common;

/// <summary>
/// Base controller that centralizes conversion of Application layer <see cref="Response{T}"/> to <see cref="IActionResult"/>.
/// One method that maps Response.StatusCode to HTTP status and returns the full Response as body.
/// Controllers only delegate to handlers and call Result(await Mediator.Send(...)).
/// </summary>
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Maps a handler result to IActionResult using StatusCode.
    /// </summary>
    protected IActionResult Result<T>(Response<T> result) =>
        StatusCode((int)(result.StatusCode ?? HttpStatusCode.OK), result);
}
