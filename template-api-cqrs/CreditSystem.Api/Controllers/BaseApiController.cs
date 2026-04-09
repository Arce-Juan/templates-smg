using Template.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Template.Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected IActionResult Result<T>(Response<T> result) =>
        StatusCode((int)(result.StatusCode ?? HttpStatusCode.OK), result);
}
