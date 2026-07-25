using Attendance.Application.Dto.Auth;
using Attendance.Shared.GenericResponse;

namespace Attendance.Application.Abstractions.Services;

public interface IIdentityService
{
    Task<GenericResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    Task<GenericResponse<AuthResponseDto>> LoginViaQrCodeAsync(string qrCode, string pin);
    Task<GenericResponse<string>> CreateUserAsync(string email, string password, string role);
    Task SeedRolesAsync();
    Task SeedDefaultAdminAsync();
    Task<GenericResponse<string>> UpdateUserEmailAsync(string oldEmail, string newEmail);
    Task<GenericResponse<string>> ChangePasswordAsync(string email, string currentPassword, string newPassword);
    Task<GenericResponse<string>> ChangePinAsync(string email, string currentPin, string newPin);
    Task<GenericResponse<string>> ResetPasswordAsync(string email, string newPassword);
    Task<GenericResponse<string>> ResetPinAsync(string email, string newPin);
}
