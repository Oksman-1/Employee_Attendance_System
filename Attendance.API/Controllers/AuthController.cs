using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto.Auth;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [AllowAnonymous] // Bypasses the [Authorize] attribute inherited from BaseController
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _identityService.LoginAsync(request);

        return ToHttpResult(response);
    }

    [HttpPost("change-password")]
    [Authorize] // Any logged-in user can change their own password
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        // For security, ensure the user can only change their own password
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value 
                     ?? User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (userEmail == null || userEmail != request.Email)
        {
            return ToHttpResult(GenericResponse<string>.Unauthorized("You are not authorized to change this password."));
        }

        var response = await _identityService.ChangePasswordAsync(request.Email, request.CurrentPassword, request.NewPassword);

        return ToHttpResult(response);
    }
}
