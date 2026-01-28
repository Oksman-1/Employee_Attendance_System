using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
[Route("hamilton-surveillance/employee-attendance-service")]
public abstract class BaseController : ControllerBase
{
    internal static int GetStatusCode(string responseCode)
    {
        return responseCode switch
        {
            "400" => StatusCodes.Status400BadRequest,
            "401" => StatusCodes.Status401Unauthorized,
            "404" => StatusCodes.Status404NotFound,
            "409" => StatusCodes.Status409Conflict,
            "500" => StatusCodes.Status500InternalServerError,
            "200" => StatusCodes.Status200OK,
            _ => Convert.ToInt32(responseCode)
        };
    }
    
    protected IActionResult ToHttpResult<T>(GenericResponse<T> response)
    {
        var statusCode = GetStatusCode(response.ResponseCode);
        return StatusCode(statusCode, response);
    }
}

