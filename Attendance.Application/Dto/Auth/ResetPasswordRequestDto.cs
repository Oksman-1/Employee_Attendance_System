using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Dto.Auth;

public class ResetPasswordRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    // Optional: If the admin wants to provide a specific temporary password.
    // If left empty, the backend will generate or use a default one.
    public string? NewPassword { get; set; }
}
