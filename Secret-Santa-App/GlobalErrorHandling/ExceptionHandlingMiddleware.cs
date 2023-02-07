using System.Net;

namespace Secret_Santa_App.GlobalErrorHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    /// <summary>
    /// Invokes the next request delegate on the Http context and catches all unhandled exceptions in the controllers.
    /// </summary>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext), "Catastrophic failure!");
        }

        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleInternalServerErrorAsync(httpContext);
        }
    }
    
    
    /// <summary>
    /// Handles unhandled exceptions and returns status code 500.
    /// </summary>
    private static async Task HandleInternalServerErrorAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsync(new ExceptionDetails
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = "Internal Server Error.",
        }.ToString());
    }
    
    
}