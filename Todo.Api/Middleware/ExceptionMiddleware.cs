using System.Net;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        
        context.Response.StatusCode = ex.GetType().ToString() switch
        {
            "DuplicateTodoException" or "InvalidTaskStatusException" =>  (int)HttpStatusCode.BadRequest,
            "NotFoundException" => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        

        var response = new { message = "An unexpected error occurred.", details = ex.Message };
        var jsonResponse = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(jsonResponse);
    }
}