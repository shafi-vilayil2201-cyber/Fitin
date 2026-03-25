using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Responses;

public static class ApiResponseFactory
{
    public static IActionResult Success<T>(T data,string message = "Success")
    {
        return new OkObjectResult(new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = null
        });
    }
    public static IActionResult Created<T>(T data, string message = "Created successfully")
    {
        return new ObjectResult(new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        })
        {StatusCode = StatusCodes.Status201Created};
    }
    public static IActionResult Failure(string message, object? errors = null, int statusCode = 400)
    {
        return new ObjectResult(new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors
        })
        {StatusCode = statusCode};
    }
}

    
