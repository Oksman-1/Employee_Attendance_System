using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Dto.Auth;

public class ChangePasswordRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; } = string.Empty;
}
