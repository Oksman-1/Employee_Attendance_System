using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Attendance.API.Extension;

/// <summary>
/// Validates incoming DTOs before the controller action executes.
/// Returns 400 if the object is null, or 422 if validation fails.
/// </summary>
public class ValidationFilterAttribute : IActionFilter
{
    public ValidationFilterAttribute()
    {
        
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"];

        // Find the DTO object among action arguments
        var param = context.ActionArguments.SingleOrDefault(x =>
            x.Value != null && x.Value.GetType().Name.EndsWith("Dto")).Value;

        // Handle null request body
        if (param is null)
        {
            context.Result = new BadRequestObjectResult(
                $"Request object is null. Controller: {controller}, Action: {action}");
            return;
        }

        // Handle validation errors
        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No implementation needed after action execution
    }
}