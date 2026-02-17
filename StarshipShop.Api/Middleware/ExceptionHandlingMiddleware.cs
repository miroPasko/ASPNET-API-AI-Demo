using System.Net;
using FluentValidation;
using StarshipShop.Api.Schemas.Responses;

namespace StarshipShop.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ErrorResponse errorResponse;
        
        switch (exception)
        {
            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = ErrorResponse.FromException(exception, context.Response.StatusCode);
                logger.LogWarning(exception, "Resource not found: {Message}", exception.Message);
                break;

            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = ErrorResponse.FromValidation(validationException.Errors);
                logger.LogWarning(exception, "Validation failed: {Message}", exception.Message);
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = ErrorResponse.FromException(exception, context.Response.StatusCode);
                logger.LogWarning(exception, "Unauthorized access: {Message}", exception.Message);
                break;

            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = ErrorResponse.FromException(exception, context.Response.StatusCode);
                logger.LogWarning(exception, "Bad request: {Message}", exception.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = ErrorResponse.FromException(exception, context.Response.StatusCode);
                logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
                break;
        }

        await context.Response.WriteAsync(errorResponse.ToJson());
    }
}
