namespace Attendance.Domain.Entities;

public class AttendanceRecord
{
    public long Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
   
    // Calendar day for which this record applies (local business day stored as SQL 'date')
    public DateOnly AttendanceDate { get; set; }
   
    // Actual event times in UTC
    public DateTimeOffset? ClockInAtUtc { get; set; }
    public DateTimeOffset? ClockOutAtUtc { get; set; }
    
    // Store in DB but can also be updated programmatically before saving
    public decimal HoursWorked { get; set; }
    public string? Notes { get; set; }
  
    // Concurrency token
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Runtime-only calculated convenience property
    public double CalculatedHoursWorked => 
        (ClockInAtUtc.HasValue && ClockOutAtUtc.HasValue)
            ? (ClockOutAtUtc.Value - ClockInAtUtc.Value).TotalHours
            : 0;
    public bool IsLate => ClockInAtUtc.HasValue && ClockInAtUtc.Value.TimeOfDay > new TimeSpan(9, 0, 0);
}