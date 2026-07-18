using Attendance.Application.Dto.Auth;
using Attendance.Shared.GenericResponse;

namespace Attendance.Application.Abstractions.Services;

public interface IIdentityService
{
    Task<GenericResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    Task<GenericResponse<string>> CreateUserAsync(string email, string password, string role);
    Task SeedRolesAsync();
}
