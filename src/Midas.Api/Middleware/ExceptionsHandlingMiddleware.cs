using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Midas.Api.Models;
using Midas.Core.Exceptions;

namespace Midas.Api.Middleware;

public class ExceptionsHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionsHandlingMiddleware> _logger;

    public ExceptionsHandlingMiddleware(ILogger<ExceptionsHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (CoreLogicException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorResponse(e.Message));
        }
        catch (NotFoundException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorResponse(e.Message));
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse(e.Message));
            LogRequest(e, context);
        }
    }

    private void LogRequest(Exception exception, HttpContext context)
    {
        if (context.Request.ContentLength > 0)
        {
            var body = new byte[context.Request.ContentLength.Value];
            _ = context.Request.Body.Read(body);
            _logger.LogError(
                exception,
                "Internal server error. Request URL: \"{0}\", request body {1}.",
                context.Request.GetDisplayUrl(),
                Encoding.UTF32.GetString(body)
            );
        }
        else
        {
            _logger.LogError(
                exception,
                "Internal server error. Request URL: \"{0}\".",
                context.Request.GetDisplayUrl()
            );
        }
    }
}