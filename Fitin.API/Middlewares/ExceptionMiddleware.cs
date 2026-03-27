
using System.Net;
using System.Text.Json;
using Fitin.API.Responses;
using Fitin.Application.Common.Exceptions;

namespace Fitin.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
       catch (Exception ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            BadRequestException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            IsSuccess = false,
            Message = ex.Message,
            Data = null,
            Errors = null
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    }
}