using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto.Auth;
using Attendance.Shared.GenericResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers;

[AllowAnonymous] // Bypasses the [Authorize] attribute inherited from BaseController
[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _identityService.LoginAsync(request);

        return ToHttpResult(response);
    }
}
