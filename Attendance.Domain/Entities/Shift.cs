namespace Attendance.Domain.Entities;

public class Shift
{
    public int Id { get; set; }
    public string Name { get; set; } = default!; // e.g., Morning, Night
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int GracePeriodMinutes { get; set; } = 10; // late allowance

    // Navigation
    public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
}