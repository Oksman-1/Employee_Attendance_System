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
    private readonly IEmailService _emailService;

    public AuthController(IIdentityService identityService, IEmailService emailService)
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    [AllowAnonymous] // Bypasses the [Authorize] attribute inherited from BaseController
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _identityService.LoginAsync(request);

        return ToHttpResult(response);
    }

    [AllowAnonymous]
    [HttpPost("login/qrcode")]
    public async Task<IActionResult> LoginViaQrCode([FromBody] LoginViaQrCodeRequestDto request)
    {
        var response = await _identityService.LoginViaQrCodeAsync(request.QrCode, request.Pin);

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

    [HttpPost("change-pin")]
    [Authorize] // Any logged-in user can change their own PIN
    public async Task<IActionResult> ChangePin([FromBody] ChangePinRequestDto request)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value 
                     ?? User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (userEmail == null)
        {
            return ToHttpResult(GenericResponse<string>.Unauthorized("You are not authorized to change this PIN."));
        }

        var response = await _identityService.ChangePinAsync(userEmail, request.CurrentPin, request.NewPin);

        return ToHttpResult(response);
    }

    [HttpPost("reset-password")]
    [Authorize(Roles = "Admin")] // Only Admins should be able to force reset another user's password
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        // If the admin didn't provide a specific new password, we'll use a strong fallback default
        var newPassword = string.IsNullOrWhiteSpace(request.NewPassword) ? "Password123!" : request.NewPassword;
        
        var response = await _identityService.ResetPasswordAsync(request.Email, newPassword);

        return ToHttpResult(response);
    }

    [HttpPost("reset-pin")]
    [Authorize(Roles = "Admin")] // Only Admins should be able to force reset another user's PIN
    public async Task<IActionResult> ResetPin([FromBody] ResetPinRequestDto request)
    {
        // If the admin didn't provide a specific new PIN, we'll generate a random 4-digit PIN
        var newPin = string.IsNullOrWhiteSpace(request.NewPin) 
            ? new Random().Next(1000, 9999).ToString() 
            : request.NewPin;
        
        var response = await _identityService.ResetPinAsync(request.Email, newPin);

        if (response.ResponseCode == "200")
        {
            var emailBody = $"Hello,\n\nYour attendance system PIN has been reset by an administrator.\n\nYour new temporary PIN is: {newPin}\n\nPlease use this to log in via your QR Code and change it immediately.\n\nBest regards,\nAdmin Team";
            
            // Fire and forget email or await it. Awaiting is safer to catch errors, but we can just fire and log.
            // Using await here
            try 
            {
                await _emailService.SendEmailAsync(request.Email, "Your New Attendance PIN", emailBody);
            }
            catch 
            {
                // We don't want to fail the whole request if email fails, but we should inform the admin
                return ToHttpResult(GenericResponse<string>.Success($"PIN reset to {newPin}, but failed to send email to user.", null, "200"));
            }
        }

        return ToHttpResult(response);
    }
}
