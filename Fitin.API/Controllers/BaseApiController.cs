using Fitin.API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;
public class BaseApiController : ControllerBase
{
    protected IActionResult Success<T> (T data,string message = "Success")
    {
        return ApiResponseFactory.Success(data,message);
    }
    protected IActionResult CreatedResponse<T>(T data,string message = "Created Successfully")
    {
        return ApiResponseFactory.Created(data,message);
    }
    protected IActionResult Failure(string message,object? errors = null, int statusCode = 400)
    {
        return ApiResponseFactory.Failure(message,errors,statusCode);
    }
}

    
