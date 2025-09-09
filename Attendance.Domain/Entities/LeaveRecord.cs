namespace Attendance.Domain.Entities;

public class LeaveRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    // Navigation
    public Employee Employee { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public bool Approved { get; set; } = false;
}