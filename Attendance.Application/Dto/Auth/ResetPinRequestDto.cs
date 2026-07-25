using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Dto.Auth;

public class ResetPinRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    // Optional: If the admin wants to provide a specific temporary PIN.
    // If left empty, the backend will use the default "1234".
    public string? NewPin { get; set; }
}
