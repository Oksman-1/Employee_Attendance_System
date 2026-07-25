using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Dto.Auth;

public class LoginViaQrCodeRequestDto
{
    [Required]
    public string QrCode { get; set; } = string.Empty;

    [Required]
    public string Pin { get; set; } = string.Empty;
}
