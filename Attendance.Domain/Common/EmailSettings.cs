using System.ComponentModel.DataAnnotations;

namespace Attendance.Domain.Common;

public class EmailSettings
{ 
    [Required] 
    public string From { get; set; } = default!;
    
    [Required] 
    public string SmtpServer { get; set; } = default!;
    
    [Range(1, 65535)] 
    public int Port { get; set; } = 587;
    
    [Required] 
    public string Username { get; set; } = default!;
    
    public string? Password { get; set; }
    
    public bool UseSSL { get; set; } = true;
}