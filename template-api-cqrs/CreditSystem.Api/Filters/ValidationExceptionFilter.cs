using Template.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Template.Api.Filters;

public sealed class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FluentValidation.ValidationException validationException)
        {
            var errors = validationException.Errors
                .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage, e.ErrorCode))
                .ToList();
            var message = string.IsNullOrWhiteSpace(validationException.Message)
                ? ErrorMessage.Validation.RequestValidationFailed
                : validationException.Message;
            var response = Response<object>.ValidationFailure(message, errors);
            context.Result = new ObjectResult(response) { StatusCode = (int)HttpStatusCode.BadRequest };
            context.ExceptionHandled = true;
        }
    }
}
