using System.Text.Json;
using FluentValidation;
using BLL.Exceptions;
using API.Controllers;

namespace API.Middleware;

public class ExceptionHandlingMiddleware(ILogger<AuthController> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = "Validation failed",
                details = validationException.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
            }));
            return;
        }

        context.Response.StatusCode = exception switch
        {
            EmailAndPasswordNullException => StatusCodes.Status400BadRequest,
            PasswordNullException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = exception.Message,
            details = (object?)null
        }));
    }
}
