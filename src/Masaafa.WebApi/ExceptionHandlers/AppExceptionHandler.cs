using Masaafa.Domain.Exceptions;
using Masaafa.WebApi.Models.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace Masaafa.WebApi.ExceptionHandlers;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not IAppException)
            return false;

        var currentException = exception as IAppException;

        httpContext.Response.StatusCode = (int)currentException!.StatusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new ApiErrorResponse
        {
            Type = currentException.Type,
            Status = (int)currentException.StatusCode,
            Title = currentException.Title,
            Detail = currentException.Detail,
            Errors = currentException.Errors
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}