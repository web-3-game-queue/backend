using GameQueue.Api.Contracts.Exceptions;
using GameQueue.Core.Exceptions;
using GameQueue.Host.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameQueue.Host.ExceptionFilters;

public class ExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;

        context.HttpContext.Response.Headers.ContentType = "text/plain; charset=utf-8";

        switch (exception)
        {
            case EntityNotFoundException e:
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsync(string.Format("Not found {0}", e.Message));
                break;

            case InvalidContentTypeException e:
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsync(string.Format("Invalid request: {0}", e.Message));
                break;

            case ValidationException e:
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsync(string.Format("Invalid request: {0}", e.Message));
                break;

            case InvalidSearchMapsRequestStatusUpdateException e:
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsync(string.Format("Invalid set status request: {0}", e.Message));
                break;

            default:
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.HttpContext.Response.WriteAsync(string.Format("Internal error: {0}", exception.Message));
                break;
        }

        await context.HttpContext.Response.CompleteAsync();

        return;
    }
}
