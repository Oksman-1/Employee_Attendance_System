using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Dto.Auth;

public class ChangePinRequestDto
{
    [Required]
    public string CurrentPin { get; set; } = string.Empty;

    [Required]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "PIN must be exactly 4 characters.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "PIN must contain only numbers.")]
    public string NewPin { get; set; } = string.Empty;
}
