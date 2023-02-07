namespace Secret_Santa_App.GlobalErrorHandling;

public static class ExceptionMiddlewareExtensions
{

    public static void ConfigureExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}