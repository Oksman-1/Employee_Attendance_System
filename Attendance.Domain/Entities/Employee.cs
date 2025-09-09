namespace Attendance.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 
    public string? Department { get; set; }
    public string JobTitle { get; set; }  = string.Empty;
    
    // Unique, URL-safe token string that we encode as a QR image in the backend
    public string QrCode { get; private set; }  = string.Empty;
    public DateTime HireDate { get; set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }  = DateTimeOffset.UtcNow;
   
    // Concurrency token (rowversion/timestamp in SQL Server)
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public bool IsActive { get; set; } = false;
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }  = new List<AttendanceRecord>();
    public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
    public ICollection<LeaveRecord> LeaveRecords { get; set; } = new List<LeaveRecord>();

    public void AssignNewQrCode(string qr)
    {
        if (string.IsNullOrWhiteSpace(qr))
            throw new ArgumentException("QR Code cannot be empty.", nameof(qr));
        
        QrCode = qr.Trim();
            
    }
}